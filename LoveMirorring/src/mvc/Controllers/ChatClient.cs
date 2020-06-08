using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Bot.Connector.DirectLine;
using Microsoft.Bot.Streaming.Transport.WebSockets;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Builder.Integration;
using System.Net.Http;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using SpotifyAPI.Web.Models;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Builder.Skills;
using Microsoft.Bot.Builder;

namespace mvc.Controllers
{
    public class ChatClient : Controller
    {

        public ChatClient()
        {
        }

        public async Task<IActionResult> Index()
        {
            try
            {           
                return View();
            }
            catch(Exception ex)
            {
                return NotFound();
            }

            
        }
    }
}
