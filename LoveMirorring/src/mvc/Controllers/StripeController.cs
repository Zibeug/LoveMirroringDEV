/*
 *      Auteur : Tim Allemann
 *      2020.04.27
 *      Permet de créer une intention de paiement
 */
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using mvc.Models;
using Newtonsoft.Json;
using Stripe;

namespace mvc.Controllers
{
    public class StripeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountController> _logger;

        public StripeController(IConfiguration configuration,
                                 ILogger<AccountController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        // GET: Stripe
        public async Task<ActionResult> Details(string subscriptionName)
        {

            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
           
            // Récurération des données et convertion des données dans le bon type
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Account/getUserInfo");
            AspNetUser user = JsonConvert.DeserializeObject<AspNetUser>(content);

            if (user == null)
            {
                return NotFound();
            }

            content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Data/userSubscription");
            List<UserSubscription> userSubscriptions = JsonConvert.DeserializeObject<List<UserSubscription>>(content);
            userSubscriptions = userSubscriptions.Where(u => u.UserId == user.Id).ToList();

            if (userSubscriptions.Count() > 0)
            {
                DateTime firstSubscriptionDate = userSubscriptions.Last().UserSubscriptionsDate;

                if (userSubscriptions.Last().Subscriptions.SubscriptionName == "1 Mois")
                {
                    firstSubscriptionDate = firstSubscriptionDate.AddMonths(1);
                }
                else if (userSubscriptions.Last().Subscriptions.SubscriptionName == "1 Année")
                {
                    firstSubscriptionDate = firstSubscriptionDate.AddYears(1);
                }

                if (firstSubscriptionDate > DateTime.Now)
                {
                    return BadRequest("Already has a subscription");
                }
            }         

            content = await client.GetStringAsync(_configuration["URLAPI"] + "api/stripe/Intent?subscriptionName=" + subscriptionName);
            PaymentIntent payment;
            try
            {
                payment = JsonConvert.DeserializeObject<PaymentIntent>(content);
            }
            catch (Exception)
            {
                return BadRequest("Either no user found or no subscription found");
            }

            ViewData["SubscriptionName"] = payment.Metadata["SubscriptionName"];
            ViewData["UserName"] = payment.Metadata["UserName"];
            ViewData["Amount"] = payment.Amount / 100;
            ViewData["ClientSecret"] = payment.ClientSecret;
            ViewData["PK"] = "pk_test_GijSVVu7BZtaY3aE9U9ACKHR00FlHg2L8Y";
            return View();
        }
       
    }
}