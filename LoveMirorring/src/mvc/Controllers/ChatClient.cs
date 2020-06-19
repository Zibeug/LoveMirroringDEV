using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Builder.Streaming;
using Microsoft.Bot.Connector.DirectLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Localization;

namespace mvc.Controllers
{
    public class ChatClient : Controller
    {
        private ILogger<ChatClient> _logger;
        public IStringLocalizer<MatchingController> _localizer;
        public ChatClient(ILogger<ChatClient> logger, IStringLocalizer<MatchingController> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var tokenClient = new DirectLineClient(new Uri("https://directline.botframework.com/"), new DirectLineClientCredentials("H-mIGKOIXJ8.M0P2_afqawnF1Yzbur8kVYgkrbaGtcoSnjP1nv11NZU"));

                tokenClient.Tokens.GenerateTokenForNewConversation();
                Conversation _conversation = await tokenClient.Conversations.StartConversationAsync().ConfigureAwait(false);

                var user = new ChannelAccount() { Id = "123", Name = "Fred" };
                var response = await tokenClient.Conversations.PostActivityAsync(_conversation.ConversationId,
                    new Activity()
                    {
                        Type = "message",
                        Text = "Hello",
                        From = user
                    }).ConfigureAwait(false);
                ActivitySet activites = await tokenClient.Conversations.GetActivitiesAsync(_conversation.ConversationId);
                this.ReceiveActivities(activites);
                return View();
            }
            catch(Exception)
            {
                return NotFound();
            }

            
        }

        public void ReceiveActivities(ActivitySet activitySet)
        {
            if (activitySet != null)
            {
                foreach (var a in activitySet.Activities)
                {
                    if (a.Type == Microsoft.Bot.Connector.DirectLine.ActivityTypes.Message && a.From.Id.Contains("bot"))
                    {
                        _logger.LogInformation($"<Bot>: {a.Text}");
                    }
                }
            }
        }
    }
}
