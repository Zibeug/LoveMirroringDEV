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
        private IHttpContextAccessor _httpContextAccessor;

        public TextBot(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            Configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
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
            string accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

            string text = null;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
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

                if (command.Contains("/ban"))
                {
                    if (command.Contains("@"))
                    {
                        string[] line = command.Split("@");
                        string nametoBan = line[1];

                        var response = await client.PutAsync(Configuration["URLAPI"] + $"api/Admin/BanUser/{nametoBan}", new StringContent(nametoBan));
                        if(response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            text = $"Utilisateur {nametoBan} banni";
                        }
                        else
                        {
                            text = $"Erreur de traitement";
                        }
                        
                        break;

                    }
                    else
                    {
                        text = "Impossible d'exécuter la commande";
                    }
                }
            }

            return text;
        }
    }
}
