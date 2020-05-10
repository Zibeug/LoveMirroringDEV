/*
 *  Auteur : Sébastien Berger
 *  Date : 12.05.2020
 *  Détail : Contrôleur pour les préférences de l'utilisateur pour optimiser et affiner ses recherches
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

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
        // GET: api/Matching/preferences
        [Route("preferences")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPreferences()
        {
            AspNetUser user = null;
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Récurération des données et convertion des données dans le bon type
            string content = await client.GetStringAsync(Configuration["URLAPI"] + "api/Account/getUserInfo");
            user = JsonConvert.DeserializeObject<AspNetUser>(content);

            try
            {
                Preference p = _context.Preferences.Where(b => b.Id == user.Id)
                    .Include(a => a.PreferenceCorpulences)
                    .Include(b => b.PreferenceHairColors)
                    .Include(c => c.PreferenceHairSizes)
                    .Include(d => d.PreferenceReligions)
                    .Include(d => d.PreferenceStyles)
                    .SingleOrDefault();
                return new JsonResult(p);
            }
            catch(Exception)
            {
                return BadRequest();
            }
        }


        //Vérifie si les les préférences on déja été enregistrées de l'utilisateur courant
        // GET : api/Matching/checkPreferences
        [Route("checkPreferences")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CheckPreference()
        {
            AspNetUser user = null;
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Récurération des données et convertion des données dans le bon type
            string content = await client.GetStringAsync(Configuration["URLAPI"] + "api/Account/getUserInfo");
            user = JsonConvert.DeserializeObject<AspNetUser>(content);

            return _context.Preferences.Where(b => b.Id == user.Id).SingleOrDefault() == null
                ? new JsonResult("error")
                : new JsonResult("success");
        }

        // Enregistre le profil de l'utilisateur s'il n'était pas enregistré au préalable
        // POST : api/Matching/SaveProfil
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

            PreferenceStyle preferenceStyle = new PreferenceStyle();
            preferenceStyle.StyleId = userChoice.StyleId;
            preferenceStyle.PreferenceId = p.PreferenceId;
            p.PreferenceStyles.Add(preferenceStyle);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return NoContent();

        }

        // Met à jour les informations de l'utilisateur s'il possède déjà des préférences enregistrées
        // POST : api/Matching/UpdateProfil
        [Route("UpdateProfil")]
        [HttpPost]
        public IActionResult UpdateProfil(UserChoiceViewModel userChoice)
        {
            try
            {
                AspNetUser user = _context.AspNetUsers.Where(b => b.UserName == userChoice.UserName).SingleOrDefault();

                Preference p = _context.Preferences.Where(b => b.Id == user.Id).Single();

                p.AgeMax = (short)userChoice.Age;
                p.SexualityId = userChoice.SexualityId;


                PreferenceCorpulence pc = _context.PreferenceCorpulences.Where(b => b.PreferenceId == p.PreferenceId).Single();
                PreferenceHairColor hc = _context.PreferenceHairColors.Where(b => b.PreferenceId == p.PreferenceId).Single();
                PreferenceHairSize hs = _context.PreferenceHairSizes.Where(b => b.PreferenceId == p.PreferenceId).Single();
                PreferenceReligion pr = _context.PreferenceReligions.Where(b => b.PreferenceId == p.PreferenceId).Single();
                PreferenceStyle ps = _context.PreferenceStyles.Where(b => b.PreferenceId == p.PreferenceId).Single();

                _context.PreferenceCorpulences.Remove(pc);
                _context.PreferenceHairSizes.Remove(hs);
                _context.PreferenceReligions.Remove(pr);
                _context.PreferenceHairColors.Remove(hc);
                _context.PreferenceStyles.Remove(ps);
                _context.SaveChanges();
            
                pc.CorpulenceId = userChoice.CorpulenceId;
                hc.HairColorId = userChoice.HairColorId;
                hs.HairSizeId = userChoice.HairSizeId;
                pr.ReligionId = userChoice.ReligionId;
                ps.StyleId = userChoice.StyleId;

                _context.PreferenceCorpulences.Add(pc);
                _context.PreferenceHairSizes.Add(hs);
                _context.PreferenceReligions.Add(pr);
                _context.PreferenceHairColors.Add(hc);
                _context.PreferenceStyles.Add(ps);

                _context.SaveChanges();
            }
            catch(Exception)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // Permet de réinitialiser le profil en cas d'erreur lors du traitement pour l'utilisateur courant
        // GET : api/Matching/Error
        [Route("Error")]
        [HttpGet]
        public async Task<IActionResult> Error()
        {
            AspNetUser user = null;
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Récurération des données et convertion des données dans le bon type
            string content = await client.GetStringAsync(Configuration["URLAPI"] + "api/Account/getUserInfo");
            user = JsonConvert.DeserializeObject<AspNetUser>(content);

            Preference p = _context.Preferences.Where(b => b.Id == user.Id).Single();
            try
            {
                if (p != null)
                {
                    _context.Preferences.Remove(p);               
                    _context.SaveChanges();
                }

                return NoContent();
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }
    }
}