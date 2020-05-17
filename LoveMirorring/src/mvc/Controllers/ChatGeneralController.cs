/*
 *      Auteur : Tim Allemann
 *      2020.05.15
 *      Renvoie la page du chat général à l'utilisateur, utilise signalR, voir le dossier Hubs classe ChatHub
 *      https://docs.microsoft.com/en-us/aspnet/core/tutorials/signalr?view=aspnetcore-3.1&tabs=visual-studio
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace mvc.Controllers
{
    [Authorize]
    public class ChatGeneralController : Controller
    {
        private readonly IConfiguration _configuration;

        public ChatGeneralController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: ChatGeneral
        public async Task<ActionResult> IndexAsync()
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/account/getUserInfo");

            AspNetUser user = JsonConvert.DeserializeObject<AspNetUser>(content);

            if (user == null)
            {
                return NotFound();
            }

            ViewData["Username"] = user.UserName;

            return View();
        }

    }
}