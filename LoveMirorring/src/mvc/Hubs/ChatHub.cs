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
    }


    public class ChatHub : Hub
    {
        private IConfiguration _configuration { get; set; }
        private DirectLineClient tokenClient;
        private Conversation _conversation;

        public ChatHub(IConfiguration configuration)
        {
            _configuration = configuration;
            tokenClient = new DirectLineClient(new Uri("https://directline.botframework.com/"), new DirectLineClientCredentials("H-mIGKOIXJ8.M0P2_afqawnF1Yzbur8kVYgkrbaGtcoSnjP1nv11NZU"));
            tokenClient.Tokens.GenerateTokenForNewConversation();
            _conversation = tokenClient.Conversations.StartConversation();
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

            AspNetUser user1 = JsonConvert.DeserializeObject<AspNetUser>(content1);
            var httpContent = new StringContent("test", Encoding.UTF8, "application/json");

            var account = new ChannelAccount() { Id = user1.Id, Name = user1.UserName };
            var response = await tokenClient.Conversations.PostActivityAsync(_conversation.ConversationId,
                new Activity()
                {
                    Type = "message",
                    Text = message,
                    From = account
                }).ConfigureAwait(false);

            ActivitySet activites = await tokenClient.Conversations.GetActivitiesAsync(_conversation.ConversationId);
            string bot = ReceiveBotActivities(activites, "lovemirroring-bot");
            await Clients.All.SendAsync("ReceiveMessage", "bot", bot);

            List<Insult> insults = JsonConvert.DeserializeObject<List<Insult>>(content);

            List<string> words = insults.Select(i => i.InsultName).ToList();

            ProfanityFilter.ProfanityFilter filter = new ProfanityFilter.ProfanityFilter();
            filter.AddProfanity(words);
            //string censored = 
            string censored = filter.CensorString(ReceiveActivities(activites, user1.UserName));

            await Clients.All.SendAsync("ReceiveMessage", user, censored);
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

            await base.OnConnectedAsync();

            ActivitySet activites = await tokenClient.Conversations.GetActivitiesAsync(_conversation.ConversationId);
            string bot = ReceiveBotActivities(activites, "lovemirroring-bot");
            await Clients.All.SendAsync("ReceiveMessage", "bot", bot);
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
            string text = "";
            if (activitySet != null)
            {
                foreach (var a in activitySet.Activities)
                {
                    if (a.Type == Microsoft.Bot.Connector.DirectLine.ActivityTypes.Message && a.From.Name.Contains(username))
                    {
                        text = a.Text;
                        break;
                    }
                }
            }
            return text;
        }

        //return list of all active connections
        public async Task GetAllActiveConnectionsAsync()
        {
            await Clients.All.SendAsync("ReceiveUser", UserHandler.UserNames.ToList());
            
        }

        private string ReceiveBotActivities(ActivitySet activitySet, string username)
        {
            string text = "";
            if (activitySet != null)
            {
                foreach (var a in activitySet.Activities)
                {
                    if (a.Type == Microsoft.Bot.Connector.DirectLine.ActivityTypes.Message && a.From.Name.Contains(username))
                    {
                        text = a.Text;
                        break;
                    }
                }
            }
            return text;
        }
    }
}
