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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using mvc.ViewModels;
using Newtonsoft.Json;
using SpotifyAPI.Web.Models;

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
            string content = await client.GetStringAsync(Configuration["URLAPI"] + "api/Spotify/SongsLiked");
            //List<Question> questionList = new List<Question>();
            List<Music> resultSong = JsonConvert.DeserializeObject<List<Music>>(content);
            ViewData["SongsLiked"] = resultSong;
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
            string content = await client.GetStringAsync(Configuration["URLAPI"] + "api/Spotify/" + input.searchSong);
            List<FullTrack> spotifyTracks = JsonConvert.DeserializeObject<List<FullTrack>>(content);

            ViewData["tracks"] = spotifyTracks;
            await Spotify();
            return View("Spotify");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SaveSong(string songname)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string json = JsonConvert.SerializeObject(songname);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            string content = await client.GetStringAsync(Configuration["URLAPI"] + "api/Spotify/CheckSong");
            PreferenceMusic pM = JsonConvert.DeserializeObject<PreferenceMusic>(content);

            HttpResponseMessage result = null;

            if (pM != null)
            {
                result = await client.PostAsync(Configuration["URLAPI"] + "api/Spotify/UpdateSong", httpContent);
            }
            else
            {
                result = await client.PostAsync(Configuration["URLAPI"] + "api/Spotify/SaveSong", httpContent);
            }
            
            
            if(result.StatusCode.Equals(StatusCodes.Status404NotFound))
            {
                ViewData["error"] = "Remplir vos préférences d'abord";
                return View("Spotify");
            }
            else
            {
                return Redirect("~/Account/Details");
            }
            
        }
    }
}