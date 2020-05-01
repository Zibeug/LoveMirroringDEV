using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using mvc.ViewModels;
using mvc.ViewModels.Admin;
using Newtonsoft.Json;

namespace mvc.Controllers
{
    public class AdminController : Controller
    {
        private IConfiguration _configuration { get; }

        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Admin/welcom");

            IndexModel overView = JsonConvert.DeserializeObject<IndexModel>(content);

            if (overView == null) 
            {
                return NotFound();
            }

            return View(overView);
        }

        public async Task<IActionResult> Search(string username)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string content = "";
            string id = "";

            if (!String.IsNullOrEmpty(username)) 
            {
                content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Admin/search/{username}");
                id = JsonConvert.DeserializeObject<string>(content);

                if (id == null)
                {
                    return NotFound();
                }
                content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Admin/user/{id}");

                SearchModel search = JsonConvert.DeserializeObject<SearchModel>(content);

                if (search == null)
                {
                    return NotFound();
                }

                return View(search);
            }

            return View();
        }

        public async Task<IActionResult> Details(string id)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (!String.IsNullOrEmpty(id))
            {
                string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Admin/user/{id}");

                SearchModel search = JsonConvert.DeserializeObject<SearchModel>(content);

                if (search == null)
                {
                    return NotFound();
                }

                return View(search);
            }

            return View("Search");
        }
    }
}