// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using EchoBot.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class TextBot : ActivityHandler
    {
        private IConfiguration Configuration { get; set; }

        public TextBot(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var replyText = "";
            if (BotCommandAsync(turnContext.Activity.Text) != null)
            {
                replyText = await BotCommandAsync(turnContext.Activity.Text);
            }
            else
            {
                replyText = $"Echo: {turnContext.Activity.Text}";
            }
            
            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Un nouvel utilisateur arrive";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }

        private async Task<string> BotCommandAsync(string command)
        {
            string text = null;
            HttpClient client = new HttpClient();
            // Récurération des données et convertion des données dans le bon type
            string content = await client.GetStringAsync(Configuration["URLAPI"] + "api/Data/BotCommands");
            List<BotCommand> botCommands = JsonConvert.DeserializeObject<List<BotCommand>>(content);

            foreach(BotCommand botCommand in botCommands)
            {
                if (botCommand.Slug.Equals(command) && !command.Equals("/help"))
                {
                    text = botCommand.Answer;
                }

                if (command.Equals("/help"))
                {
                    text += "List des commandes : ";
                    foreach (BotCommand c in botCommands)
                    {
                        text += c.Slug + " ";
                    }
                    break;
                }
            }

            return text;
        }
    }
}
