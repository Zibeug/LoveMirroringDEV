using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using mvc.Models;
using mvc.Services;
using mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace mvc.Hubs
{
    public class LetsChatHub : Hub
    {
        private List<ConnectionPC> _connectionPCs;

        public LetsChatHub()
        {
            _connectionPCs = ConnectionsSingleton.GetConnectionList();
        }

        public async Task SendMessage(string username, string friendname, string message, string connId)
        {

            AddConnection(username, friendname, connId);

            string connectionFriendId = _connectionPCs
                                            .Where(c => c.username == friendname)
                                            .OrderByDescending(c => c.dateConnection)
                                            .Select(c => c.connectionId)
                                            .FirstOrDefault();

            if (connectionFriendId != null && connectionFriendId != "")
            {
                await Clients.Client(connectionFriendId).SendAsync("ReceiveMessage", username, message);
            }

            string connectionUserId = _connectionPCs
                                            .Where(c => c.username == username)
                                            .OrderByDescending(c => c.dateConnection)
                                            .Select(c => c.connectionId)
                                            .FirstOrDefault();

            if (connectionUserId != null && connectionUserId != "")
            {
                await Clients.Client(connectionUserId).SendAsync("ReceiveMessage", username, message);
            }

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
