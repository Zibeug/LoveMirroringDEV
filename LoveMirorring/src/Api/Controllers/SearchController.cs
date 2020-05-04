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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private LoveMirroringContext _context;
        private IConfiguration Configuration { get; set; }

        public SearchController(LoveMirroringContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        [Route("search")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> OnPostSearchAsync()
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

            if (user != null)
            {
                List<Sex> Sexes = _context.Sexes.ToList();
                List<Profil> Profils = _context.Profils.ToList();

            }
            Preference pref = _context.Preferences.Where(b => b.Id == user.Id).Single();
            PreferenceCorpulence prefCorp = _context.PreferenceCorpulences.Where(b => b.PreferenceId == pref.PreferenceId).Single();
            PreferenceReligion prefRel = _context.PreferenceReligions.Where(b => b.PreferenceId == pref.PreferenceId).Single();
            PreferenceHairColor prefHC = _context.PreferenceHairColors.Where(b => b.PreferenceId == pref.PreferenceId).Single();
            PreferenceHairSize prefHS = _context.PreferenceHairSizes.Where(b => b.PreferenceId == pref.PreferenceId).Single();
            UserProfil userProf = _context.UserProfils.Where(b => b.Id == user.Id).Single();

            if (pref == null)
            {
                throw new Exception("Enregistrez vos préférences d'abord");
            }
            string SexeName = "";
            if(pref.SexualityId == 1) //Hetero
            {
                if (user.Sexe.SexeName.Equals("Homme")) // Si c'est un homme il cherche
                {
                    SexeName = "Femme";
                }
                else // Si c'est une femme il cherche
                {
                    SexeName = "Homme";
                }
            }
            else // Homo
            {
                SexeName = user.Sexe.SexeName;
            }

            var allUsersId = from u in await _context.AspNetUsers.ToListAsync() select u.Id;
            var allUsersLikeId = from us in await _context.UserLikes.ToListAsync() select us.Id1;
            var allUsersNotLike = allUsersId.Except(allUsersLikeId);

            var userProfils = _context.UserProfils.Where(d => d.ProfilId == userProf.ProfilId).Select(d => d.Id).ToList();
            
            IEnumerable<MatchingModel> usersChoices = _context.AspNetUsers
                            .Include(a => a.Sexe)
                            .Where(p => userProfils.Contains(p.Id))
                            .Where(s => s.Sexe.SexeName.Equals(SexeName))
                            .Where(d => DateTime.Now.Year - d.Birthday.Year <= pref.AgeMax)
                            .Where(d => allUsersNotLike.Contains(d.Id))
                            .Where(d => d.CorpulenceId == prefCorp.CorpulenceId)
                            .Where(d => d.HairSizeId == prefHS.HairSizeId)
                            .Where(d => d.HairColorId == prefHC.HairColorId)
                            .Select(u => new MatchingModel() { 
                                Id = u.Id, 
                                UserName = u.UserName, 
                                Age = DateTime.Now.Year - u.Birthday.Year, 
                                Sexe = u.Sexe.SexeName, 
                                Profil = userProf.Profil.ProfilName,
                                Corpulence = u.Corpulence.CorpulenceName,
                                HairColor = u.HairColor.HairColorName,
                                HairSize = u.HairSize.HairSizeName,
                            });

            return new JsonResult(usersChoices);
        }

        [Route("Like")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Like([FromBody]string id)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            AspNetUser user = null;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string userString = await client.GetStringAsync(Configuration["URLAPI"] + "api/Account/getUserInfo");
            user = JsonConvert.DeserializeObject<AspNetUser>(userString);
            UserLike ul = new UserLike();

            ul.Id = user.Id;
            ul.Id1 = id;
            try
            {
                _context.UserLikes.Add(ul);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Route("GetLike")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetLike()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            AspNetUser user = null;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string userString = await client.GetStringAsync(Configuration["URLAPI"] + "api/Account/getUserInfo");
            user = JsonConvert.DeserializeObject<AspNetUser>(userString);

            List<UserLike> ul = _context.UserLikes.Where(d => d.Id == user.Id).ToList();
            List<AspNetUser> userList = new List<AspNetUser>();

            foreach(UserLike u in ul)
            {
                userList.Add(_context.AspNetUsers.Where(d => d.Id == u.Id1).Single());
            }

            return new JsonResult(userList);
        }
    }
}