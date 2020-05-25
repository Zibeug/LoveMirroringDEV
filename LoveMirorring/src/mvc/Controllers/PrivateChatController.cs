using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using mvc.ViewModels.Chat;
using Newtonsoft.Json;
using Unosquare.Swan;

namespace mvc.Controllers
{
    public class PrivateChatController : Controller
    {

        private IConfiguration Configuration { get; set; }

        public PrivateChatController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<IActionResult> IndexAsync()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string search = await client.GetStringAsync(Configuration["URLAPI"] + "api/Search/GetLike");
            List<AspNetUser> userList = JsonConvert.DeserializeObject<List<AspNetUser>>(search);
            ViewData["UserList"] = userList;

            return View();
        }

        public async Task<IActionResult> ChatAsync(string id)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            
            string searchTalk = await client.GetStringAsync(Configuration["URLAPI"] + $"api/PrivateChat/GetTalk/{id}");
            Talk talk = JsonConvert.DeserializeObject<Talk>(searchTalk);

            string searchMessages = await client.GetStringAsync(Configuration["URLAPI"] + $"api/PrivateChat/GetMessages/{talk.TalkId}");
            IEnumerable<GetMessagesViewModel> messages = JsonConvert.DeserializeObject<IEnumerable<GetMessagesViewModel>>(searchMessages);

            string content = await client.GetStringAsync(Configuration["URLAPI"] + "api/account/getUserInfo");
            AspNetUser user = JsonConvert.DeserializeObject<AspNetUser>(content);

            CreateMessageViewModel chat = new CreateMessageViewModel { Talk = talk,UserId = user.Id, DisplayMessages = messages, UserLikedId = id };

            return View(chat);
        }

        public async Task<IActionResult> CreateMessageAsync(CreateMessageViewModel chat)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (ModelState.IsValid)
            {
                StringContent httpContent = new StringContent(chat.NewMessage.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(Configuration["URLAPI"] + "api/PrivateChat/CreateMessage", httpContent);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return BadRequest();
                }

                return RedirectToAction("Chat", "PrivateChat", new { id = chat.UserLikedId });
            }

            return RedirectToAction(nameof(IndexAsync));
        }

    }
}