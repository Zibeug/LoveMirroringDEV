using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [Route("subscription")]
        [HttpGet]
        public IActionResult GetSubscription()
        {
            List<Subscription> responses = _context.Subscriptions.ToList();
            return new JsonResult(responses);
        }
    }
}