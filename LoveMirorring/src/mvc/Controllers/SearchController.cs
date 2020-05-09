using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using mvc.ViewModels;
using Newtonsoft.Json;

namespace mvc.Controllers
{
    public class SearchController : Controller
    {
        private IConfiguration Configuration { get; set; }

        public SearchController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            return View("Search");
        }

        [Authorize]
        public async Task<IActionResult> Search()
        {
            try
            {
                string accessToken = await HttpContext.GetTokenAsync("access_token");

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                string search = await client.GetStringAsync(Configuration["URLAPI"] + "api/Search/search");
                IEnumerable<MatchingModel> searchResult = JsonConvert.DeserializeObject<IEnumerable<MatchingModel>>(search);
                ViewData["Search"] = searchResult;
                return View("Search");
            }
            catch (Exception)
            {
                return View("Error");
            }
            
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Like(string id)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri(Configuration["URLAPI"] + "api/Search/Like");
            string json = JsonConvert.SerializeObject(id);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync(client.BaseAddress, httpContent);
            var responseString = response.Result;
            return View("Like");
        }

        [Authorize]
        public async Task<IActionResult> Like()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string search = await client.GetStringAsync(Configuration["URLAPI"] + "api/Search/GetLike");
            List<AspNetUser> userList = JsonConvert.DeserializeObject<List<AspNetUser>>(search);
            ViewData["UserList"] = userList;

            return View("Like");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UnLike(string id)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri(Configuration["URLAPI"] + "api/Search/" + id);
            var response = client.DeleteAsync(client.BaseAddress);
            var responseString = response.Result;
            return View("Search");
        }
    }
}