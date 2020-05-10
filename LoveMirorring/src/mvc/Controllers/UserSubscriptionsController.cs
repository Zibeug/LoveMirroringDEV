/*
 *      Auteur : Tim Allemann
 *      2020.04.27
 *      Permet de vérifier ses abonnements et ou choisir un abonnement
 */

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
    public class UserSubscriptionsController : Controller
    {
        private readonly IConfiguration _configuration;

        public UserSubscriptionsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: UserSubscriptions
        public async Task<IActionResult> Index()
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/account/getUserInfo");

            AspNetUser user = JsonConvert.DeserializeObject<AspNetUser>(content);

            if (user == null)
            {
                return NotFound();
            }

            content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Data/userSubscription");
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
                else
                {
                    ViewData["FirstSubscriptionDate"] = firstSubscriptionDate.ToString("dd MMMM yyyy");
                }
            }

            return View(userSubscriptions);
        }

    }
}
