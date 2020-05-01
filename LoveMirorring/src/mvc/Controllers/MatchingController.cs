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
    public class MatchingController : Controller
    {
        private HttpClient client = new HttpClient();
        private IConfiguration Configuration { get; }

        public MatchingController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [Authorize]
        public async Task<IActionResult> MatchingAsync()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string sexes = await client.GetStringAsync(Configuration["URLAPI"] + "api/Data/Sex");
            string religions = await client.GetStringAsync(Configuration["URLAPI"] + "api/Data/religions");
            string corpulences = await client.GetStringAsync(Configuration["URLAPI"] + "api/Data/corpulences");
            string username = await client.GetStringAsync(Configuration["URLAPI"] + "api/Account/getUserInfo");
            string preferences = await client.GetStringAsync(Configuration["URLAPI"] + "api/Matching/checkPreferences");
            string hairSize = await client.GetStringAsync(Configuration["URLAPI"] + "api/Data/hairSize");
            string hairColor = await client.GetStringAsync(Configuration["URLAPI"] + "api/Data/hairColor");
            string sexuality = await client.GetStringAsync(Configuration["URLAPI"] + "api/Data/sexuality");

            List<Sex> resultSexes = JsonConvert.DeserializeObject<List<Sex>>(sexes);
            List<Religion> resultReligions = JsonConvert.DeserializeObject<List<Religion>>(religions);
            List<Corpulence> resultCorpulences = JsonConvert.DeserializeObject<List<Corpulence>>(corpulences);
            List<HairColor> resultHairColors = JsonConvert.DeserializeObject<List<HairColor>>(hairColor);
            List<HairSize> resultHairSizes = JsonConvert.DeserializeObject<List<HairSize>>(hairSize);
            List<Sexuality> resultSexualities = JsonConvert.DeserializeObject<List<Sexuality>>(sexuality);

            AspNetUser user = JsonConvert.DeserializeObject<AspNetUser>(username);
            string resultPreferences = JsonConvert.DeserializeObject<string>(preferences);

            ViewData["PrefenresCheck"] = resultPreferences;
            //List<Question> questionList = new List<Question>();
            ViewData["sexes"] = resultSexes;
            ViewData["religions"] = resultReligions;
            ViewData["corpulences"] = resultCorpulences;
            ViewData["username"] = user.UserName;
            ViewData["hairColor"] = resultHairColors;
            ViewData["hairSize"] = resultHairSizes;
            ViewData["sexuality"] = resultSexualities;

            return View();
        }

        [Authorize]
        public async Task<IActionResult> UpdateProfil()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            await MatchingAsync();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string preferences = null;
            try
            {
                preferences = await client.GetStringAsync(Configuration["URLAPI"] + "api/Matching/preferences");
            }
            catch(HttpRequestException)
            {
                return View("Error");
            }
            
            
            Preference p = new Preference();
            p = JsonConvert.DeserializeObject<Preference>(preferences);

            ViewData["preferences"] = p;

            ViewData["PrefenresCheck"] = "success";

            

            return View("Matching");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SaveProfil(UserChoiceViewModel userChoice)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri(Configuration["URLAPI"] + "api/Matching/SaveProfil");
            string json = await Task.Run(() => JsonConvert.SerializeObject(userChoice));
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync(client.BaseAddress, httpContent);
            var responseString = response.Result;
            ViewData["PrefenresCheck"] = "error";
            return View("Matching");

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdatedProfil(UserChoiceViewModel userChoice)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri(Configuration["URLAPI"] + "api/Matching/UpdateProfil");
            string json = await Task.Run(() => JsonConvert.SerializeObject(userChoice));
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync(client.BaseAddress, httpContent);
            var responseString = response.Result;
            ViewData["PrefenresCheck"] = "error";
            return View("Matching");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Error()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri(Configuration["URLAPI"] + "api/Matching/Error");
            var response = client.GetAsync(client.BaseAddress);
            return await MatchingAsync();
        }   
    }
}