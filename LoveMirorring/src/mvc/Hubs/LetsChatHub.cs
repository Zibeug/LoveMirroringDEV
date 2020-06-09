using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using mvc.Models;
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
        private List<ConnectionPC> _ConnectionPCs;

        public LetsChatHub(List<ConnectionPC> connectionPCs)
        {
            _ConnectionPCs = connectionPCs;
        }

        public async Task SendMessage(string username, string friendname, string message, string connId)
        {

            if (!_ConnectionPCs.Any(c => c.connectionId == connId))
            {
                _ConnectionPCs.Add(
                    new ConnectionPC
                    {
                        connectionId = Context.ConnectionId,
                        username = username,
                        friendname = friendname,
                        dateConnection = DateTime.Now
                    }
                );
            }

            string connectionFriendId = _ConnectionPCs
                                            .Where(c => c.username == friendname)
                                            .OrderByDescending(c => c.dateConnection)
                                            .Select(c => c.connectionId)
                                            .FirstOrDefault();

            if (connectionFriendId != null && connectionFriendId != "")
            {
                await Clients.Client(connectionFriendId).SendAsync("ReceiveMessage", username, message);
            }

            string connectionUserId = _ConnectionPCs
                                            .Where(c => c.username == username)
                                            .OrderByDescending(c => c.dateConnection)
                                            .Select(c => c.connectionId)
                                            .FirstOrDefault();

            if (connectionUserId != null && connectionUserId != "")
            {
                await Clients.Client(connectionUserId).SendAsync("ReceiveMessage", username, message);
            }

        }
    }
}
