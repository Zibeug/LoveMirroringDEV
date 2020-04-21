﻿using System;
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
using mvc.ViewModels;
using Newtonsoft.Json;

namespace mvc.Controllers
{
    public class SpotifyController : Controller
    {
        private HttpClient client = new HttpClient();
        private IConfiguration Configuration { get; }

        public SpotifyController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [Authorize]
        public async Task<IActionResult> CategorieAsync()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string content = await client.GetStringAsync(Configuration["URLAPI"] + "api/Spotify/categories");
            //List<Question> questionList = new List<Question>();
            List<SpotifyItem> resultCategories = JsonConvert.DeserializeObject<List<SpotifyItem>>(content);
            ViewData["categories"] = resultCategories;
            return View("Spotify");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SearchSong(string id)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Configuration["URLAPI"] + "api/Spotify/SearchSong?id=" + id);
                HttpContent c = new StringContent("OK", Encoding.UTF8, "application/json");
                var responseTask = client.PostAsync(client.BaseAddress, c);
                responseTask.Wait();

                var result = responseTask.Result;
            }
            return View("Spotify");
        }
    }
}