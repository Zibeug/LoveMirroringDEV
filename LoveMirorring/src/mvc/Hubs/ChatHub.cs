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
    }


    public class ChatHub : Hub
    {
        private HubConnection connection;
        private IConfiguration _configuration { get; set; }

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
            var httpContent = new StringContent("test", Encoding.UTF8, "application/json");
            //var bot = await client.PostAsync("http://localhost:3978/api/messages", httpContent);
            List<Insult> insults = JsonConvert.DeserializeObject<List<Insult>>(content);

            List<string> words = insults.Select(i => i.InsultName).ToList();

            ProfanityFilter.ProfanityFilter filter = new ProfanityFilter.ProfanityFilter();
            filter.AddProfanity(words);
            string censored = filter.CensorString(message);

            await Clients.All.SendAsync("ReceiveMessage", user, censored);
        }

        public override Task OnConnectedAsync()
        {
            UserHandler.ConnectedIds.Add(Context.ConnectionId);
            return base.OnConnectedAsync();
        }
    }
}
