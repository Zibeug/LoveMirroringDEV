/*
 * Auteur : Sébastien Berger
 * Date : 12.05.2020
 * Description : Contrôleur pour pouvoir accéder à Spotify et ses fonctionnalités
 */﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Api.Models;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using Microsoft.Extensions.Configuration;
using SpotifyAPI.Web.Models;
using SpotifyAPI.Web.Enums;
using Api.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SpotifyController : ControllerBase
    {
        private readonly LoveMirroringContext _context;
        private SpotifyWebAPI _spotify;
        private CredentialsAuth auth;
        private Token token;
        private IConfiguration Configuration { get; }

        public SpotifyController(LoveMirroringContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
            auth = new CredentialsAuth(Configuration["ClientID"], Configuration["ClientSecret"]);
        }

        // Permet de récupérer une musique par son ID
        // GET : api/Spotify/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSong(string id)
        {
            token = await auth.GetToken();
            _spotify = new SpotifyWebAPI()
            {
                AccessToken = token.AccessToken,
                TokenType = token.TokenType
            };

            SearchItem songs = await _spotify.SearchItemsAsync(id, SearchType.Track);
            List<FullTrack> listSongs = new List<FullTrack>();

            foreach(FullTrack song in songs.Tracks.Items)
            {
                listSongs.Add(song);
            }
            return new JsonResult(listSongs);

        }

        //Permet de récupérer les sons qui ont déjà été likés par des utilisateurs
        // GET: api/Spotify/SongsLiked
        [Route("SongsLiked")]
        [HttpGet]
        public async Task<IActionResult> GetSongsLiked()
        {
            List<Music> musics = await _context.Musics.ToListAsync();

            return new JsonResult(musics);
        }

        // Permet d'enregistrer la préférence de l'utilisateur
        // POST: api/Spotify/SaveSong
        [Route("SaveSong")]
        [HttpPost]
        public async Task<IActionResult> SaveSong([FromBody]string songname)
        {
            try
            {
                AspNetUser user = null;
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Récurération des données et convertion des données dans le bon type
                string content = await client.GetStringAsync(Configuration["URLAPI"] + "api/Account/getUserInfo");
                user = JsonConvert.DeserializeObject<AspNetUser>(content);

                string[] song = songname.Split('-');
                Music music = new Music();
                music.MusicName = song[0];
                music.ArtistName = song[1];

                Music search = _context.Musics.Where(x => x.MusicName.Equals(music.MusicName)).SingleOrDefault();

                if (search == null)
                {
                    _context.Musics.Add(music);
                    _context.SaveChanges();
                }
                else
                {
                    music = search;
                }

                Preference p = _context.Preferences
                    .Include(p => p.PreferenceMusics)
                    .Include(p => p.PreferenceHairSizes)
                    .Include(p => p.PreferenceHairColors)
                    .Include(p => p.PreferenceCorpulences)
                    .Include(p => p.PreferenceReligions)
                    .Include(p => p.PreferenceStyles)
                    .Where(x => x.Id == user.Id)
                    .SingleOrDefault();

                if (p != null)
                {
                    PreferenceMusic pM = new PreferenceMusic();
                    pM.MusicId = music.MusicId;
                    pM.PreferenceId = p.PreferenceId;
                    p.PreferenceMusics.Add(pM);

                    _context.SaveChanges();
                    return NoContent();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}