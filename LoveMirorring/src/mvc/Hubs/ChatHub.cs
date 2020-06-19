/*
 *      Auteur : Tim Allemann
 *      2020.05.15
 *      The ChatHub class inherits from the SignalR Hub class. The Hub class manages connections, groups, and messaging.
 *      The SendMessage method can be called by a connected client to send a message to all clients. 
 *      JavaScript client code that calls the method is shown later in the tutorial. SignalR code is asynchronous to provide maximum scalability.
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Bot.Connector.DirectLine;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace mvc.Hubs
{
    public static class UserHandler
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();
        public static HashSet<string> UserNames = new HashSet<string>();
        public static string ConversationId;
        public static List<ChannelAccount> channelAccount = new List<ChannelAccount>();
        public static DirectLineClient tokenClient;
    }


    public class ChatHub : Hub
    {
        private IConfiguration _configuration { get; set; }
        private Conversation _conversation;

        public ChatHub(IConfiguration configuration)
        {
            _configuration = configuration; 
        }

        public async Task SendMessage(string user, string message)
        {
            string accessToken = await Context.GetHttpContext().GetTokenAsync("access_token");
            // Préparation de l'appel à l'API
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            // Récurération des données et convertion des données dans le bon type
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Insults");
            string content1 = await client.GetStringAsync(_configuration["URLAPI"] + "api/account/getUserInfo");
            string content2 = await client.GetStringAsync(_configuration["URLAPI"] + "api/Data/BotCommands");
            List<BotCommand> botCommands = JsonConvert.DeserializeObject<List<BotCommand>>(content2);

            AspNetUser user1 = JsonConvert.DeserializeObject<AspNetUser>(content1);
            var httpContent = new StringContent("test", Encoding.UTF8, "application/json");

            var response = await UserHandler.tokenClient.Conversations.PostActivityAsync(UserHandler.ConversationId,
                new Activity()
                {
                    Type = "message",
                    Text = message,
                    From = UserHandler.channelAccount.Where(x => x.Name.Equals(user1.UserName)).FirstOrDefault(),
                    
                }).ConfigureAwait(false);


            ActivitySet activites = await UserHandler.tokenClient.Conversations.GetActivitiesAsync(UserHandler.ConversationId);
            List<Insult> insults = JsonConvert.DeserializeObject<List<Insult>>(content);
            List<string> words = insults.Select(i => i.InsultName).ToList();

            
            ProfanityFilter.ProfanityFilter filter = new ProfanityFilter.ProfanityFilter();
            filter.AddProfanity(words);
            //string censored = 
            string censored = filter.CensorString(ReceiveActivities(activites, user1.UserName));

            await Clients.All.SendAsync("ReceiveMessage", user, censored);

            ActivitySet botActivites = await UserHandler.tokenClient.Conversations.GetActivitiesAsync(UserHandler.ConversationId);
            string bot = null;
            if (message.Contains("/giphy"))
            {
                bot = ReceiveBotActivities(botActivites, "lovemirroring-bot");
                await Clients.All.SendAsync("ImageReceive", "bot", bot);
            }
            else
            {
                bot = ReceiveBotActivities(botActivites, "lovemirroring-bot");
                await Clients.All.SendAsync("ReceiveMessage", "bot", bot);
            }
        }

        public override async Task OnConnectedAsync()
        {
            string accessToken = await Context.GetHttpContext().GetTokenAsync("access_token");
            // Préparation de l'appel à l'API
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            // Récurération des données et convertion des données dans le bon type
            string content1 = await client.GetStringAsync(_configuration["URLAPI"] + "api/account/getUserInfo");

            AspNetUser user1 = JsonConvert.DeserializeObject<AspNetUser>(content1);

            UserHandler.ConnectedIds.Add(Context.ConnectionId);
            UserHandler.UserNames.Add(user1.UserName);
            UserHandler.tokenClient = new DirectLineClient(new Uri("https://directline.botframework.com/"), new DirectLineClientCredentials("H-mIGKOIXJ8.M0P2_afqawnF1Yzbur8kVYgkrbaGtcoSnjP1nv11NZU"));
            await UserHandler.tokenClient.Tokens.GenerateTokenForNewConversationAsync();
            _conversation = await UserHandler.tokenClient.Conversations.StartConversationAsync();
            UserHandler.channelAccount.Add(new ChannelAccount() { Id = user1.Id, Name = user1.UserName });

            UserHandler.ConversationId = _conversation.ConversationId;

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string accessToken = await Context.GetHttpContext().GetTokenAsync("access_token");
            // Préparation de l'appel à l'API
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            // Récurération des données et convertion des données dans le bon type
            string content1 = await client.GetStringAsync(_configuration["URLAPI"] + "api/account/getUserInfo");

            AspNetUser user1 = JsonConvert.DeserializeObject<AspNetUser>(content1);

            UserHandler.ConnectedIds.Remove(Context.ConnectionId);
            UserHandler.UserNames.Remove(user1.UserName);
            await base.OnDisconnectedAsync(exception);

        }

        private string ReceiveActivities(ActivitySet activitySet, string username)
        {
            List<Activity> list = new List<Activity>();
            string text = "";
            if (activitySet != null)
            {
                foreach (var a in activitySet.Activities)
                {
                    if (a.Type == Microsoft.Bot.Connector.DirectLine.ActivityTypes.Message && a.From.Name.Equals(username))
                    {
                        list.Add(a);
                    }
                }
            }

            text = list.Where(x => x.From.Name == username).OrderByDescending(f => f.Timestamp).First().Text;
            return text;
        }

        //return list of all active connections
        public async Task GetAllActiveConnectionsAsync()
        {
            await Clients.All.SendAsync("ReceiveUser", UserHandler.UserNames.ToList());
            
        }

        private string ReceiveBotActivities(ActivitySet activitySet, string username)
        {
            List<Activity> list = new List<Activity>();
            string text = "";
            if (activitySet != null)
            {
                foreach (var a in activitySet.Activities)
                {
                    if (a.Type == Microsoft.Bot.Connector.DirectLine.ActivityTypes.Message && a.From.Name.Contains(username))
                    {
                        list.Add(a);
                    }
                }
            }

            text = list.Where(x => x.From.Name == username).OrderByDescending(f => f.Timestamp).First().Text;
            return text;
        }
    }
}
