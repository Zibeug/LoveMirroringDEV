using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private LoveMirroringContext _context;

        public SearchController(LoveMirroringContext context)
        {
            _context = context;
        }

        [Route("search")]
        [HttpPost]
        [Authorize]
        public async Task<JsonResult> OnPostSearchAsync()
        {
            AspNetUser user = null;
            user = _context.AspNetUsers.Find(User);

            if (user != null)
            {
                List<Sex> Sexes = _context.Sexes.ToList();
                List<Profil> Profils = _context.Profils.ToList();

            }
            Preference pref = _context.Preferences.Where(b => b.Id == user.Id).Single();
            UserProfil userProf = _context.UserProfils.Where(b => b.Id == user.Id).Single();

            if (pref == null)
            {
                throw new Exception("Enregistrez vos préférences d'abord");
            }
            string SexeName = "";
            if(pref.SexualityId == 1)
            {
                if (user.Sexe.SexeName.Equals("Homme"))
                {
                    SexeName = "Femme";
                }
                else
                {
                    SexeName = "Homme";
                }
            }
            else
            {
                SexeName = user.Sexe.SexeName;
            }
            var allUsersId = from u in await _context.AspNetUsers.ToListAsync() select u.Id;
            var allUsersLikeId = from us in await _context.UserLikes.ToListAsync() select us.Id1;
            var allUsersNotLike = allUsersId.Except(allUsersLikeId);


            var usersChoices = from u in await _context.AspNetUsers.ToListAsync()
                               where DateTime.Now.Year - u.Birthday.Year <= pref.AgeMax
                               && u.Id != user.Id && allUsersNotLike.Contains(u.Id)
                               join s in await _context.Sexes.ToListAsync() on u.SexeId equals s.SexeId
                               where s.SexeName.Equals(SexeName)
                               join up in await _context.UserProfils.ToListAsync() on u.Id equals up.Id
                               join p in await _context.Profils.ToListAsync() on up.ProfilId equals p.ProfilId
                               where p.ProfilId.Equals(userProf.ProfilId)
                               select new MatchingModel { UserName = u.UserName, Age = DateTime.Now.Year - u.Birthday.Year, SexeId = s.SexeId, ProfilId = p.ProfilId }; ;

            IEnumerable<MatchingModel> matchingResult = usersChoices;

            return new JsonResult(matchingResult);

            //exemple retrouver like https://localhost:44365/Like/parisa@lol.ch
        }
    }
}