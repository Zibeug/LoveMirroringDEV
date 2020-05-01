using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchingController : ControllerBase
    {
        private readonly LoveMirroringContext _context;
        private IConfiguration Configuration { get; set; }

        public MatchingController(LoveMirroringContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        // Récupére les informations de l'utiliateur courant
        // GET: api/preferences
        [Route("preferences")]
        [HttpGet]
        [Authorize]
        public IActionResult GetPreferences()
        {
            AspNetUser user = null;
            foreach(var claim in User.Claims)
            {
                if(user == null)
                {
                    user = _context.AspNetUsers.Find(claim.Value);
                }
            }

            try
            {
                Preference p = _context.Preferences.Where(b => b.Id == user.Id).SingleOrDefault();
                p.PreferenceCorpulences.Add(_context.PreferenceCorpulences.Where(b => b.PreferenceId == p.PreferenceId).Single());
                p.PreferenceReligions.Add(_context.PreferenceReligions.Where(b => b.PreferenceId == p.PreferenceId).Single());
                p.PreferenceHairColors.Add(_context.PreferenceHairColors.Where(b => b.PreferenceId == p.PreferenceId).Single());
                p.PreferenceHairSizes.Add(_context.PreferenceHairSizes.Where(b => b.PreferenceId == p.PreferenceId).Single());
                return new JsonResult(p);
            }
            catch(Exception)
            {
                return BadRequest();
            }
        }


        //Vérifie si les les préférences on déja été enregistrées de l'utilisateur courant
        // GET : api/checkPreferences
        [Route("checkPreferences")]
        [HttpGet]
        [Authorize]
        public IActionResult CheckPreference()
        {
            AspNetUser user = null;
            foreach (var claim in User.Claims)
            {
                if (user == null)
                {
                    user = _context.AspNetUsers.Find(claim.Value);
                }
            }

            return _context.Preferences.Where(b => b.Id == user.Id).FirstOrDefault() != null
                ? new JsonResult("error")
                : new JsonResult("success");
        }

        // Enregistre le profil de l'utilisateur s'il n'était pas enregistré au préalable
        // POST : api/SaveProfil
        [Route("SaveProfil")]
        [HttpPost]
        public IActionResult SaveProfil(UserChoiceViewModel userChoice)
        {
            Preference p = new Preference();
            p.AgeMax = (short)userChoice.Age;
            p.AgeMin = 18;

            AspNetUser user = _context.AspNetUsers.Where(b => b.UserName == userChoice.UserName).SingleOrDefault();

            p.Id = user.Id;
            p.SexualityId = userChoice.SexualityId;
            _context.Preferences.Add(p);

            PreferenceReligion prefReligion = new PreferenceReligion();
            prefReligion.ReligionId = userChoice.ReligionId;
            prefReligion.PreferenceId = p.PreferenceId;
            p.PreferenceReligions.Add(prefReligion);

            PreferenceCorpulence prefCorpulence = new PreferenceCorpulence();
            prefCorpulence.CorpulenceId = userChoice.CorpulenceId;
            prefCorpulence.PreferenceId = p.PreferenceId;
            p.PreferenceCorpulences.Add(prefCorpulence);

            PreferenceHairColor preferenceHairColor = new PreferenceHairColor();
            preferenceHairColor.HairColorId = userChoice.HairColorId;
            preferenceHairColor.PreferenceId = p.PreferenceId;
            p.PreferenceHairColors.Add(preferenceHairColor);

            PreferenceHairSize preferenceHairSize = new PreferenceHairSize();
            preferenceHairSize.HairSizeId = userChoice.HairSizeId;
            preferenceHairSize.PreferenceId = p.PreferenceId;
            p.PreferenceHairSizes.Add(preferenceHairSize);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok();

        }

        // Met à jour les informations de l'utilisateur s'il possède déjà des préférences enregistrées
        // POST : api/UpdateProfil
        [Route("UpdateProfil")]
        [HttpPost]
        public IActionResult UpdateProfil(UserChoiceViewModel userChoice)
        {
            AspNetUser user = _context.AspNetUsers.Where(b => b.UserName == userChoice.UserName).SingleOrDefault();

            Preference p = _context.Preferences.Where(b => b.Id == user.Id).Single();

            p.AgeMax = (short)userChoice.Age;
            p.SexualityId = userChoice.SexualityId;


            PreferenceCorpulence pc = _context.PreferenceCorpulences.Where(b => b.PreferenceId == p.PreferenceId).Single();
            PreferenceHairColor hc = _context.PreferenceHairColors.Where(b => b.PreferenceId == p.PreferenceId).Single();
            PreferenceHairSize hs = _context.PreferenceHairSizes.Where(b => b.PreferenceId == p.PreferenceId).Single();
            PreferenceReligion pr = _context.PreferenceReligions.Where(b => b.PreferenceId == p.PreferenceId).Single();

            _context.PreferenceCorpulences.Remove(pc);
            _context.PreferenceHairSizes.Remove(hs);
            _context.PreferenceReligions.Remove(pr);
            _context.PreferenceHairColors.Remove(hc);
            _context.SaveChanges();
            
            pc.CorpulenceId = userChoice.CorpulenceId;
            hc.HairColorId = userChoice.HairColorId;
            hs.HairSizeId = userChoice.HairSizeId;
            pr.ReligionId = userChoice.ReligionId;

            _context.PreferenceCorpulences.Add(pc);
            _context.PreferenceHairSizes.Add(hs);
            _context.PreferenceReligions.Add(pr);
            _context.PreferenceHairColors.Add(hc);
            try
            {
                _context.SaveChanges();
            }
            catch(Exception)
            {
                return BadRequest();
            }
            return Ok();
        }

        // Permet de réinitialiser le profil en cas d'erreur lors du traitement pour l'utilisateur courant
        // GET : api/Error
        [Route("Error")]
        [HttpGet]
        public IActionResult Error()
        {
            AspNetUser user = null;
            string id = "";

            try
            {
                // Il faut utiliser le Claim pour retrouver l'identifiant de l'utilisateur
                id = User.Claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").SingleOrDefault().Value;
                user = _context.AspNetUsers.Where(b => b.Id == id).SingleOrDefault();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            Preference p = _context.Preferences.Where(b => b.Id == user.Id).Single();
            if(p != null)
            {
                _context.Preferences.Remove(p);
                _context.SaveChanges();
            }
            

            return Ok();
        }
    }
}