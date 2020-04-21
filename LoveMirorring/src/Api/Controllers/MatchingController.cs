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
            Religion religion = _context.Religions.Where(b => b.ReligionName == userChoice.Religion).FirstOrDefault();
            Corpulence corpulence = _context.Corpulences.Where(b => b.CorpulenceName == userChoice.Corpulence).FirstOrDefault();
            AspNetUser user = _context.AspNetUsers.Where(b => b.UserName == userChoice.UserName).FirstOrDefault();

            p.Id = user.Id;
            p.SexualityId = 1;
            _context.Preferences.Add(p);
            PreferenceReligion prefReligion = new PreferenceReligion();
            prefReligion.ReligionId = religion.ReligionId;
            prefReligion.PreferenceId = p.PreferenceId;
            p.PreferenceReligions.Add(prefReligion);
            PreferenceCorpulence prefCorpulence = new PreferenceCorpulence();
            prefCorpulence.CorpulenceId = corpulence.CorpulenceId;
            prefCorpulence.PreferenceId = p.PreferenceId;
            p.PreferenceCorpulences.Add(prefCorpulence);

            if(_context.Preferences.Find(user.Id) == null)
            {
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
            
        }
    }
}