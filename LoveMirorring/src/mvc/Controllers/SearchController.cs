using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string search = await client.GetStringAsync(Configuration["URLAPI"] + "api/Search/search");

            IEnumerable<MatchingModel> searchResult = JsonConvert.DeserializeObject<IEnumerable<MatchingModel>>(search);

            ViewData["Search"] = searchResult;

            return View("Search");
        }
    }
}