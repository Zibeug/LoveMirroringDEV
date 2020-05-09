/*
 * Auteur : Sébastien Berger 
 * Date : 10.05.2020
 * Description : Contrôleur pour afficher les actions possibles avec Spotify
 */
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
using mvc.ViewModels;
using Newtonsoft.Json;

namespace mvc.Controllers
{
    public class SpotifyController : Controller
    {
        private IConfiguration Configuration { get; }

        public SpotifyController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // Permet d'afficher les catégories de Spotify
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Spotify()
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

        // Permet de cherche une musique en fonction de la catégorie choisie
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SearchSong(SpotifyInput input)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string content = await client.GetStringAsync(Configuration["URLAPI"] + "api/Spotify/" + input.favoriteCategory);
            List<SpotifyTrack> spotifyTracks = JsonConvert.DeserializeObject<List<SpotifyTrack>>(content);

            return View("Spotify");
        }
    }
}