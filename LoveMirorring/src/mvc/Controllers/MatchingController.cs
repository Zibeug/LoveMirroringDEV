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
using mvc.Models;
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
            string sexes = await client.GetStringAsync(Configuration["URLAPI"] + "api/Matching/Sex");
            string religions = await client.GetStringAsync(Configuration["URLAPI"] + "api/Matching/religions");
            string corpulences = await client.GetStringAsync(Configuration["URLAPI"] + "api/Matching/corpulences");
            string profils = await client.GetStringAsync(Configuration["URLAPI"] + "api/Matching/profils");
            List<Sex> resultSexes = JsonConvert.DeserializeObject<List<Sex>>(sexes);
            List<Religion> resultReligions = JsonConvert.DeserializeObject<List<Religion>>(religions);
            List<Corpulence> resultCorpulences = JsonConvert.DeserializeObject<List<Corpulence>>(corpulences);
            List<Profil> resultProfils = JsonConvert.DeserializeObject<List<Profil>>(profils);
            //List<Question> questionList = new List<Question>();
            ViewData["sexes"] = resultSexes;
            ViewData["religions"] = resultReligions;
            ViewData["corpulences"] = resultCorpulences;
            ViewData["profils"] = resultProfils;
            return View();
        }
    }
}