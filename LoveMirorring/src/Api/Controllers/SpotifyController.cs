﻿/*
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

namespace Api.Controllers
{
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

        // Permet de retourner les catégories de Spotify
        // GET : api/Spotify/categories
        [Route("categories")]
        [HttpGet]
        public async Task<IActionResult> GetCategories(string type)
        {
            token = await auth.GetToken();
            _spotify = new SpotifyWebAPI()
            {
                AccessToken = token.AccessToken,
                TokenType = token.TokenType
            };

            var categories = _spotify.GetCategories("FR");
            var item = categories.Categories.Items;
            return new JsonResult(item);
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

            var song = _spotify.SearchItems(id, SearchType.Track);
            return new JsonResult(song);

        }
    }
}