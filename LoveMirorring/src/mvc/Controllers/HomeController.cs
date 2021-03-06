﻿/*
 * Auteur : Sébastien Berger, Tim Allemann
 * Date : 31.05.2020
 * Description : permet d'afficher la page d'accueil
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using mvc.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private IConfiguration Configuration { get; }

        public IStringLocalizer<HomeController> _localizer;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IStringLocalizer<HomeController> localizer)
        {
            _logger = logger;
            Configuration = configuration;
            _localizer = localizer;
        }

        [Authorize]
        public async Task<IActionResult> IndexAsync()
        {
            // Préparation de l'appel à l'API
            string accessToken = "";
            try
            {
                accessToken = await HttpContext.GetTokenAsync("access_token");
            }
            catch (UnauthorizedAccessException)
            {
                return SignOut("Cookies", "oidc");
            }
            catch (Exception)
            {
                // Si pour une raison ou pour une la récupération du token ne fonctionne pas déconnecter l'utilisateur également
                return SignOut("Cookies", "oidc");
            }
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string content = await client.GetStringAsync(Configuration["URLAPI"] + "api/account/getUserInfo");

            AspNetUser user = JsonConvert.DeserializeObject<AspNetUser>(content);

            content = await client.GetStringAsync(Configuration["URLAPI"] + "api/Home/GetAds");

            List<Ad> ads = JsonConvert.DeserializeObject<List<Ad>>(content);

            if (user != null && !user.AccountCompleted)
            {
                return Redirect("Account/Details/");
            }
            else
            {
                if(user.UserSubscriptions.Count() == 0)
                {
                    ViewData["img"] = this.RandomPicture(ads);
                    ViewData["URLAPI"] = Configuration["URLAPI"];
                    ViewData["link"] = ads.Where(x => x.AdView.Equals(ViewData["img"] as string)).SingleOrDefault().Link;

                }

                return View();
            }
            
        }

        [Authorize]
        public async Task<IActionResult> CallApi()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var content = await client.GetStringAsync(Configuration["URLIdentityServer4"] + "identity");

            ViewBag.Json = JArray.Parse(content).ToString();
            return View("json");
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /*
         * Auteur : Sébastien Berger 
         * Date 29.05.2020
         * Description : Permet de récupérer une publicité de façon aléatoire.
         */
        private string RandomPicture(List<Ad> ads)
        {
            Ad[] tabAd = ads.ToArray();
            Random rand = new Random();
            int nb = rand.Next(1, ads.Count() + 1);

            return tabAd[nb - 1].AdView;
        }
    }
}
