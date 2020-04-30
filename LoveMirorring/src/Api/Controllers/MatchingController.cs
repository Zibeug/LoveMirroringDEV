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

        [Route("sex")]
        [HttpGet]
        public IActionResult GetSex()
        {
            List<Sex> responses = _context.Sexes.ToList();
            return new JsonResult(responses);
        }

        [Route("corpulences")]
        [HttpGet]
        public IActionResult GetCorpulences()
        {
            List<Corpulence> responses = _context.Corpulences.ToList();
            return new JsonResult(responses);
        }

        [Route("religions")]
        [HttpGet]
        public IActionResult GetReligion()
        {
            List<Religion> responses = _context.Religions.ToList();
            return new JsonResult(responses);
        }

        [Route("profils")]
        [HttpGet]
        public IActionResult GetProfils()
        {
            List<Profil> responses = _context.Profils.ToList();
            return new JsonResult(responses);
        }

        [Route("hairSize")]
        [HttpGet]
        public IActionResult GetHairSize()
        {
            List<HairSize> responses = _context.HairSizes.ToList();
            return new JsonResult(responses);
        }

        [Route("sexuality")]
        [HttpGet]
        public IActionResult GetSexuality()
        {
            List<Sexuality> responses = _context.Sexualities.ToList();
            return new JsonResult(responses);
        }

        [Route("hairColor")]
        [HttpGet]
        public IActionResult GetHairColor()
        {
            List<HairColor> responses = _context.HairColors.ToList();
            return new JsonResult(responses);
        }

        [Route("user")]
        [HttpGet]
        [Authorize]
        public IActionResult GetUsername()
        {
            AspNetUser user = null;
            foreach(var claim in User.Claims)
            {
                if(user == null)
                {
                    user = _context.AspNetUsers.Find(claim.Value);
                }
            }
            return new JsonResult(user.UserName);
        }

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