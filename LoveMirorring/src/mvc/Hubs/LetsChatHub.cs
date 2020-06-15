using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using mvc.Models;
using mvc.Services;
using mvc.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unosquare.Swan;

namespace mvc.Hubs
{
    public class LetsChatHub : Hub
    {
        private List<ConnectionPC> _connectionPCs;
        private IConfiguration _configuration { get; set; }

        public LetsChatHub(IConfiguration configuration)
        {
            _connectionPCs = ConnectionsSingleton.GetConnectionList();
            _configuration = configuration;
        }

        public async Task SendMessage(string username, string userId, string friendname, string message, string connId, string talkId)
        {

            AddConnection(username, friendname, connId);

            string accessToken = await Context.GetHttpContext().GetTokenAsync("access_token");
            // Préparation de l'appel à l'API
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string insultsRequest = await client.GetStringAsync(_configuration["URLAPI"] + "api/Insults");
            List<Insult> insults = JsonConvert.DeserializeObject<List<Insult>>(insultsRequest);

            List<string> words = insults.Select(i => i.InsultName).ToList();

            ProfanityFilter.ProfanityFilter filter = new ProfanityFilter.ProfanityFilter();
            filter.AddProfanity(words);
            //string censored = 
            string censored = filter.CensorString(message);

            Message messageDB = new Message
            {
                MessageDate = DateTime.Now,
                MessageText = censored,
                Id = userId,
                TalkId = short.Parse(talkId)
            };


            string connectionFriendId = _connectionPCs
                                            .Where(c => c.username == friendname)
                                            .OrderByDescending(c => c.dateConnection)
                                            .Select(c => c.connectionId)
                                            .FirstOrDefault();

            if (connectionFriendId != null && connectionFriendId != "")
            {
                await Clients.Client(connectionFriendId).SendAsync("ReceiveMessage", username, censored);
            }

            string connectionUserId = _connectionPCs
                                            .Where(c => c.username == username)
                                            .OrderByDescending(c => c.dateConnection)
                                            .Select(c => c.connectionId)
                                            .FirstOrDefault();

            if (connectionUserId != null && connectionUserId != "")
            {
                await Clients.Client(connectionUserId).SendAsync("ReceiveMessage", username, censored);
            }

           

           
            StringContent httpContent = new StringContent(messageDB.ToJson(), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(_configuration["URLAPI"] + "api/PrivateChat/CreateMessage", httpContent);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _connectionPCs = _connectionPCs.Where(c => c.dateConnection.AddHours(1) >= DateTime.Now).ToList();
        }
        public async Task AddConnection(string username, string friendname, string connId)
        {

            if (!_connectionPCs.Any(c => c.connectionId == connId))
            {
                _connectionPCs.Add(
                    new ConnectionPC
                    {
                        connectionId = Context.ConnectionId,
                        username = username,
                        friendname = friendname,
                        dateConnection = DateTime.Now
                    }
                );
            }

        }
    }
}
