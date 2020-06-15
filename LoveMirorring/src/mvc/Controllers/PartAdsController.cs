using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using Newtonsoft.Json;

namespace mvc.Controllers
{
    public class PartAdsController : Controller
    {
        private IConfiguration Configuration { get; set; }
        public PartAdsController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string content = await client.GetStringAsync(Configuration["URLAPI"] + "api/account/getUserInfo");

            AspNetUser user = JsonConvert.DeserializeObject<AspNetUser>(content);

            content = await client.GetStringAsync(Configuration["URLAPI"] + "api/Home/GetAds");

            List<Ad> ads = JsonConvert.DeserializeObject<List<Ad>>(content);

            if (user.UserSubscriptions.Count() == 0)
            {
                ViewData["img"] = this.RandomPicture(ads);
                ViewData["URLAPI"] = Configuration["URLAPI"];
                ViewData["link"] = ads.Where(x => x.AdView.Equals(ViewData["img"] as string)).SingleOrDefault().Link;

            }

            return PartialView("PartAds");
        }

        private string RandomPicture(List<Ad> ads)
        {
            Ad[] tabAd = ads.ToArray();
            Random rand = new Random();
            int nb = rand.Next(1, ads.Count() + 1);

            return tabAd[nb - 1].AdView;
        }
    }
}