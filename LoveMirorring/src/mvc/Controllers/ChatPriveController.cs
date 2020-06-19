/*
 * Auteur : Allemann Tim
 * Date : 16.06.2020
 * Description : Contrôleur permettant d'afficher un chat privé
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using mvc.Models;
using mvc.ViewModels.Chat;
using Newtonsoft.Json;

namespace mvc.Controllers
{
    public class ChatPriveController : Controller
    {
        private readonly IConfiguration _configuration;
        public IStringLocalizer<MatchingController> _localizer;

        public ChatPriveController(IConfiguration configuration, IStringLocalizer<MatchingController> localizer)
        {
            _configuration = configuration;
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LetsChatAsync(string friendName, string friendId)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string searchTalk = await client.GetStringAsync(_configuration["URLAPI"] + $"api/PrivateChat/GetTalk/{friendId}");
            Talk talk = JsonConvert.DeserializeObject<Talk>(searchTalk);

            string searchMessages = await client.GetStringAsync(_configuration["URLAPI"] + $"api/PrivateChat/GetMessages/{talk.TalkId}");
            IEnumerable<GetMessagesViewModel> messages = JsonConvert.DeserializeObject<IEnumerable<GetMessagesViewModel>>(searchMessages);

            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/account/getUserInfo");
            AspNetUser user = JsonConvert.DeserializeObject<AspNetUser>(content);

            ViewData["username"] = user.UserName;
            ViewData["friendname"] = friendName;
            ViewData["talkId"] = talk.TalkId;
            ViewData["userId"] = user.Id;
            ViewData["messages"] = messages.OrderByDescending(m => m.Date).Take(10);
            return View();
        }
    }
}
