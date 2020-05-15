﻿/*
 * Auteur : Sébastien Berger 
 * Date : 10.05.2020
 * Description : Contrôleur pour effectuer des recherches à partir de la vue et liker des utilisateurs
 * 
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
using mvc.Models;
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

        public async Task<IActionResult> IndexAsync()
        {
            /*
                 * Auteur : Tim Allemann 
                 * Date : 15.05.2020
                 * Description : vérifie si l'utilisateur a un abonnement ou pas
                 * 
                 */
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string content = await client.GetStringAsync(Configuration["URLAPI"] + "api/account/getUserInfo");
            AspNetUser user = JsonConvert.DeserializeObject<AspNetUser>(content);

            if (user == null)
            {
                return NotFound();
            }

            content = await client.GetStringAsync(Configuration["URLAPI"] + "api/Data/userSubscription");
            List<UserSubscription> userSubscriptions = JsonConvert.DeserializeObject<List<UserSubscription>>(content);
            userSubscriptions = userSubscriptions.Where(u => u.UserId == user.Id).ToList();

            if (userSubscriptions.Count() == 0)
            {
                ViewData["HasSubscription"] = false;
            }
            else
            {
                ViewData["HasSubscription"] = true;
                DateTime firstSubscriptionDate = userSubscriptions.Last().UserSubscriptionsDate;

                if (userSubscriptions.Last().Subscriptions.SubscriptionName == "1 Mois")
                {
                    firstSubscriptionDate = firstSubscriptionDate.AddMonths(1);
                }
                else if (userSubscriptions.Last().Subscriptions.SubscriptionName == "1 Année")
                {
                    firstSubscriptionDate = firstSubscriptionDate.AddYears(1);
                }

                if (firstSubscriptionDate < DateTime.Now)
                {
                    ViewData["HasSubscription"] = false;
                }
            }

            return View("Search");
        }

        // Retourne la vue avec les profils qui correspondent
        [Authorize]
        public async Task<IActionResult> Search()
        {
            try
            {
                string accessToken = await HttpContext.GetTokenAsync("access_token");

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                string search = await client.GetStringAsync(Configuration["URLAPI"] + "api/Search/search");
                IEnumerable<MatchingModel> searchResult = JsonConvert.DeserializeObject<IEnumerable<MatchingModel>>(search);
                ViewData["Search"] = searchResult;

                /*
                 * Auteur : Tim Allemann 
                 * Date : 15.05.2020
                 * Description : vérifie si l'utilisateur a un abonnement ou pas
                 * 
                 */
                string content = await client.GetStringAsync(Configuration["URLAPI"] + "api/account/getUserInfo");
                AspNetUser user = JsonConvert.DeserializeObject<AspNetUser>(content);

                if (user == null)
                {
                    return NotFound();
                }

                content = await client.GetStringAsync(Configuration["URLAPI"] + "api/Data/userSubscription");
                List<UserSubscription> userSubscriptions = JsonConvert.DeserializeObject<List<UserSubscription>>(content);
                userSubscriptions = userSubscriptions.Where(u => u.UserId == user.Id).ToList();

                if (userSubscriptions.Count() == 0)
                {
                    ViewData["HasSubscription"] = false;
                }
                else
                {
                    ViewData["HasSubscription"] = true;
                    DateTime firstSubscriptionDate = userSubscriptions.Last().UserSubscriptionsDate;

                    if (userSubscriptions.Last().Subscriptions.SubscriptionName == "1 Mois")
                    {
                        firstSubscriptionDate = firstSubscriptionDate.AddMonths(1);
                    }
                    else if (userSubscriptions.Last().Subscriptions.SubscriptionName == "1 Année")
                    {
                        firstSubscriptionDate = firstSubscriptionDate.AddYears(1);
                    }

                    if (firstSubscriptionDate < DateTime.Now)
                    {
                        ViewData["HasSubscription"] = false;
                    }
                }

                return View("Search");
            }
            catch (Exception)
            {
                return View("Error");
            }
            
        }

        // Permet de liker un utilisateur
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Like(string username)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri(Configuration["URLAPI"] + "api/Search/Like");
            string json = JsonConvert.SerializeObject(username);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync(client.BaseAddress, httpContent);
            var responseString = response.Result;
            return View("Like");
        }

        // Retourne les likes de l'utilisateur
        [Authorize]
        public async Task<IActionResult> Like()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string search = await client.GetStringAsync(Configuration["URLAPI"] + "api/Search/GetLike");
            List<AspNetUser> userList = JsonConvert.DeserializeObject<List<AspNetUser>>(search);
            ViewData["UserList"] = userList;

            return View("Like");
        }

        // Permet d'enlever son like d'un utilisateur
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UnLike(string username)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri(Configuration["URLAPI"] + "api/Search/UnLike/" + username);
            var response = client.DeleteAsync(client.BaseAddress);
            var responseString = response.Result;
            return View("Search");
        }

        // Retourne les détails de l'utilisateur choisi dans la recherche ou dans les likes
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Details(string username)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri(Configuration["URLAPI"] + "api/Search/UserDetails/" + username);
            string userToDisplay = await client.GetStringAsync(client.BaseAddress);
            string userList = await client.GetStringAsync(Configuration["URLAPI"] + "api/Search/GetLike");
            string userMatch = await client.GetStringAsync(Configuration["URLAPI"] + "api/Search/CheckMatch/" + username);

            string userMatched = JsonConvert.DeserializeObject<string>(userMatch);
            List<AspNetUser> userLike = JsonConvert.DeserializeObject<List<AspNetUser>>(userList);
            AspNetUser user = JsonConvert.DeserializeObject<AspNetUser>(userToDisplay);

            ViewData["userLike"] = "NotLike";
            foreach (AspNetUser temp in userLike)
            {
                if (temp.Id == user.Id)
                {
                    ViewData["userLike"] = "Like";
                }
            }

            ViewData["match"] = userMatched;

            return View(user);
        }
    }
}