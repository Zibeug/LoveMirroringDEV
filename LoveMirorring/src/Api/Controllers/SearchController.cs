/*
 * Auteur : Sébastien Berger
 * Date : 07.05.2020
 * Détail : Contrôleur pour effectuer une recherche d'un ou plusieurs utilisateurs en fonction de ses préférences
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
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string checkPref = await client.GetStringAsync(Configuration["URLAPI"] + "api/Matching/checkPreferences");

            string result = JsonConvert.DeserializeObject<string>(checkPref);

            if (result.Equals("error"))
            {
                return BadRequest();
            }

            try
            {
                // Il faut utiliser le Claim pour retrouver l'identifiant de l'utilisateur
                id = User.Claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").SingleOrDefault().Value;
                user = _context.AspNetUsers.Where(b => b.Id == id).SingleOrDefault();

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
                PreferenceStyle prefStyle = _context.PreferenceStyles.Where(b => b.PreferenceId == pref.PreferenceId).Include(a => a.Style).Single();
                UserProfil userProf = _context.UserProfils.Where(b => b.Id == user.Id).Single();

                if (pref == null)
                {
                    throw new Exception("Enregistrez vos préférences d'abord");
                }

                string SexeName = "";
                if (pref.SexualityId == 1) //Hetero
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
                var userStyles = _context.UserStyles.Where(d => d.StyleId == prefStyle.StyleId).Select(d => d.Id).ToList();

                IEnumerable<MatchingModel> usersChoices = _context.AspNetUsers
                                .Include(a => a.Sexe)
                                .Include(a => a.Religion)
                                .Where(p => userProfils.Contains(p.Id))
                                .Where(s => s.Sexe.SexeName.Equals(SexeName))
                                .Where(d => DateTime.Now.Year - d.Birthday.Year <= pref.AgeMax)
                                .Where(d => allUsersNotLike.Contains(d.Id))
                                .Where(d => d.CorpulenceId == prefCorp.CorpulenceId)
                                .Where(d => d.HairSizeId == prefHS.HairSizeId)
                                .Where(d => d.HairColorId == prefHC.HairColorId)
                                .Where(p => userStyles.Contains(p.Id))
                                .Select(u => new MatchingModel()
                                {
                                    Id = u.Id,
                                    UserName = u.UserName,
                                    Age = DateTime.Now.Year - u.Birthday.Year,
                                    Sexe = u.Sexe.SexeName,
                                    Profil = userProf.Profil.ProfilName,
                                    Corpulence = u.Corpulence.CorpulenceName,
                                    HairColor = u.HairColor.HairColorName,
                                    HairSize = u.HairSize.HairSizeName,
                                    Style = prefStyle.Style.StyleName,
                                    Religion = u.Religion.ReligionName
                                });

                return new JsonResult(usersChoices);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSearch(string id)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            AspNetUser currentUser = null;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string userString = await client.GetStringAsync(Configuration["URLAPI"] + "api/Account/getUserInfo");
            currentUser = JsonConvert.DeserializeObject<AspNetUser>(userString);

            AspNetUser user = _context.AspNetUsers.Where(d => d.Id == id).Single();

            if(user == null)
            {
                return BadRequest();
            }
            else
            {
                UserLike userLike = _context.UserLikes.Where(d => d.Id == currentUser.Id && d.Id1 == user.Id).Single();
                try
                {
                    _context.UserLikes.Remove(userLike);
                    _context.SaveChanges();
                    return Ok();
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
        }
    }
}