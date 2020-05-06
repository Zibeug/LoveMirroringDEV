/*
 *      Auteur : Tim Allemann
 *      2020.04.27
 *      Permet de créer une intention de paiement avec Stripe
 */
 
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StripeController : ControllerBase
    {
        private readonly LoveMirroringContext _LoveMirroringcontext;
        private IConfiguration Configuration { get; set; }

        public StripeController(LoveMirroringContext context,
                                IConfiguration configuration)
        {
            _LoveMirroringcontext = context;
            Configuration = configuration;
        }

        // Renvoie une intention de paiement
        // GET: api/Stripe/Intent
        [Route("Intent")]
        [HttpGet()]
        public async Task<IActionResult> GetIntent(string subscriptionName)
        {
            string userId = "";
            try
            {
                // Il faut utiliser le Claim pour retrouver l'identifiant de l'utilisateur
                userId = User.Claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").SingleOrDefault().Value;
            }
            catch (Exception)
            {
                return BadRequest("No user found");
            }

            AspNetUser user = _LoveMirroringcontext.AspNetUsers.Where(u => u.Id == userId).SingleOrDefault();

            Models.Subscription subscription = await _LoveMirroringcontext.Subscriptions.
                                            Where(s => s.SubscriptionName == subscriptionName).
                                            SingleOrDefaultAsync();
            if (subscription != null)
            {
                StripeConfiguration.ApiKey = Configuration["Stripe:ApiKey"];

                PaymentIntentCreateOptions options = new PaymentIntentCreateOptions
                {
                    // 10 chf = 1000 dans Stripe
                    Amount = (long?)(subscription.SubscriptionPrice*100),
                    Currency = "chf",
                    ReceiptEmail = user.Email,
                    // Verify your integration in this guide by including this parameter
                    Metadata = new Dictionary<string, string>
                {
                  { "integration_check", "accept_a_payment" },
                },
                };
                options.Metadata.Add("SubscriptionName", subscription.SubscriptionName);
                options.Metadata.Add("UserName", user.Firstname + " " + user.LastName);
                options.Metadata.Add("UserId", userId);

                PaymentIntentService service = new PaymentIntentService();
                PaymentIntent paymentIntent = service.Create(options);

                return Ok(paymentIntent);
            }
            else
            {
                return BadRequest("No subscription price available");
            }          
        }

    }
}