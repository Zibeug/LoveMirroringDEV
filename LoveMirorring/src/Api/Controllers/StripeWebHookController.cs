/*
 *      Auteur : Tim Allemann
 *      2020.04.27
 *      Permet de recevoir des données du fournisseur Stripe
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeWebHookController : ControllerBase
    {
        // If you are testing your webhook locally with the Stripe CLI you
        // can find the endpoint's secret by running `stripe listen`
        // Otherwise, find your endpoint's secret in your webhook settings in the Developer Dashboard
        const string endpointSecret = "whsec_TL9NhBB2yR4wvF11CCucwwm7g7Eemz2w";
        //const string endpointSecret = "whsec_7NQYMceYVRhNDSkU1P8iurW51JUZqP2U";

        private readonly LoveMirroringContext _LoveMirroringcontext;

        public StripeWebHookController(LoveMirroringContext context)
        {
            _LoveMirroringcontext = context;
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);

                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    Console.WriteLine("PaymentIntent was successful!");
                }
                else if (stripeEvent.Type == Events.PaymentMethodAttached)
                {
                    var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
                    Console.WriteLine("PaymentMethod was attached to a Customer!");
                }
                else if (stripeEvent.Type == Events.ChargeSucceeded)
                {
                    
                    Charge charge = stripeEvent.Data.Object as Charge;
                    short subscriptionId = _LoveMirroringcontext.Subscriptions.
                                            Where(s => s.SubscriptionPrice == (charge.Amount/100)).
                                            Select(s => s.SubscriptionId).
                                            SingleOrDefault();

                    UserSubscription subscription = new UserSubscription { 
                        UserId = charge.Metadata["UserId"],
                        UserSubscriptionsAmount = charge.Amount / 100,
                        UserSubscriptionsDate = DateTime.Now,
                        SubscriptionsId = subscriptionId
                    };

                    _LoveMirroringcontext.UserSubscriptions.Add(subscription);
                    await _LoveMirroringcontext.SaveChangesAsync();
                }
                // ... handle other event types
                else
                {
                    // Unexpected event type
                    return BadRequest();
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    }
}