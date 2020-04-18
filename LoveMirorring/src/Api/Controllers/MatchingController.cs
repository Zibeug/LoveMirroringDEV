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

        [Route("SaveProfil")]
        [HttpPost]
        public IActionResult SaveProfil(UserChoiceViewModel userChoice)
        {
            Preference p = new Preference();
            p.AgeMax = (short)userChoice.Age;
            p.AgeMin = 18;
            Religion religion = _context.Religions.Where(b => b.ReligionName == userChoice.Religion).FirstOrDefault();
            AspNetUser user = _context.AspNetUsers.Where(b => b.UserName == userChoice.UserName).FirstOrDefault();
            p.Id = user.Id;
            p.SexualityId = 1;
            _context.Preferences.Add(p);

            try
            {
                _context.SaveChanges();
                PreferenceReligion prefReligion = new PreferenceReligion();
                prefReligion.ReligionId = religion.ReligionId;
                prefReligion.PreferenceId = p.PreferenceId;
                p.PreferenceReligions.Add(prefReligion);
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }
    }
}