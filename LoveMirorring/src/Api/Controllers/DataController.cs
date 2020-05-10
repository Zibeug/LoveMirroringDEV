/*
 * Auteurs : Sébastien Berger, Tim Allemann 
 * Date : 09.05.2020
 * Description : permet de récupérer plusieurs données pour les exploiter dans le profil, les préférences et l'inscription
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly LoveMirroringContext _context;

        public DataController(LoveMirroringContext context)
        {
            _context = context;
        }

        // Permet de récupérer une liste de sexes
        // GET : api/Data/sex
        [Route("sex")]
        [HttpGet]
        public IActionResult GetSex()
        {
            List<Sex> responses = _context.Sexes.ToList();
            return new JsonResult(responses);
        }

        // Permet de récupérer une liste de corpulences
        // GET : api/Data/corpulences
        [Route("corpulences")]
        [HttpGet]
        public IActionResult GetCorpulences()
        {
            List<Corpulence> responses = _context.Corpulences.ToList();
            return new JsonResult(responses);
        }

        // Permet de récupérer une liste de religions
        // GET : api/Data/religions
        [Route("religions")]
        [HttpGet]
        public IActionResult GetReligion()
        {
            List<Religion> responses = _context.Religions.ToList();
            return new JsonResult(responses);
        }

        // Permet de récupérer une liste de profils
        // GET : api/Data/profils
        [Route("profils")]
        [HttpGet]
        public IActionResult GetProfils()
        {
            List<Profil> responses = _context.Profils.ToList();
            return new JsonResult(responses);
        }

        // Permet de récupérer une liste de taille de cheveux
        // GET : api/Data/hairSize
        [Route("hairSize")]
        [HttpGet]
        public IActionResult GetHairSize()
        {
            List<HairSize> responses = _context.HairSizes.ToList();
            return new JsonResult(responses);
        }

        // Permet de récupérer une liste de sexualités
        // GET : api/Data/sexuality
        [Route("sexuality")]
        [HttpGet]
        public IActionResult GetSexuality()
        {
            List<Sexuality> responses = _context.Sexualities.ToList();
            return new JsonResult(responses);
        }

        // Permet de récupérer une liste de couleurs de cheveux
        // GET : api/Data/hairColor
        [Route("hairColor")]
        [HttpGet]
        public IActionResult GetHairColor()
        {
            List<HairColor> responses = _context.HairColors.ToList();
            return new JsonResult(responses);
        }

        // Permet de récupérer une liste de souscriptions à un abonnement
        // GET : api/Data/subscription
        [Route("subscription")]
        [HttpGet]
        public IActionResult GetSubscription()
        {
            List<Subscription> responses = _context.Subscriptions.ToList();
            return new JsonResult(responses);
        }

        // Permet de récupérer une liste de styles vestimentaires
        // GET : api/Data/styles
        [Route("styles")]
        [HttpGet]
        public IActionResult GetStyles()
        {
            List<Style> responses = _context.Styles.ToList();
            return new JsonResult(responses);
        }
        
        //Permet de récupérer la liste des abonnements
        // GET : api/Data/userSubscription
        [Route("userSubscription")]
        [HttpGet]
        public IActionResult GetUserSubscription()
        {
            List<UserSubscription> responses = _context.UserSubscriptions
                                                    .Include(s => s.Subscriptions)
                                                    .ToList();
            return new JsonResult(responses);
        }
    }
}