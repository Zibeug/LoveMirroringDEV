/*
 *      Auteur : Tim Allemann
 *      2020.04.27
 *      Permet de gérer ses données profils
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stripe;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly LoveMirroringContext _context;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AccountController> _logger;
        private readonly IActionContextAccessor _accessor;

        public AccountController(LoveMirroringContext context,
                                 IEmailSender emailSender,
                                 ILogger<AccountController> logger,
                                 IActionContextAccessor accessor)
        {
            _context = context;
            _emailSender = emailSender;
            _logger = logger;
            _accessor = accessor;
        }

        // Renvoie les données de l'utilisateur
        // GET: api/Account/5
        [Route("getUserInfo")]
        [HttpGet()]
        public async Task<ActionResult<AspNetUser>> GetAspNetUser()
        {
            AspNetUser user = null;
            string id = "";

            try
            {
                // Il faut utiliser le Claim pour retrouver l'identifiant de l'utilisateur
                id = User.Claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").SingleOrDefault().Value;
            }
            catch (Exception)
            {
                return BadRequest();
            }

            user = await _context.AspNetUsers
                            .Include(a => a.Corpulence)
                            .Include(a => a.HairColor)
                            .Include(a => a.HairSize)
                            .Include(a => a.Sexe)
                            .Include(a => a.Sexuality)
                            .Include(a => a.Subscription)
                            .Include(a => a.UserStyles)
                                .ThenInclude(a => a.Style)
                            .Include(a => a.UserSubscriptions)
                            .Include(a => a.UserTraces)
                            .Include(a => a.Religion)
                            .Include(a => a.Pictures)
                            .SingleOrDefaultAsync(a => a.Id == id);

            if (user == null)
            {
                return BadRequest();
            }
            else
            {
                return user;
            }

        }

        // Met à jour les données de l'utilisateur
        // PUT: api/Account/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Route("PutUser")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAspNetUser(AspNetUser aspNetUser)
        {
            
            _context.Entry(aspNetUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUserExists(aspNetUser.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Sébastien Berger
        // Met à jour les données de l'utilisateur
        // PUT: api/Account/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Route("PutStyle")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStyle(UserStyle userStyle)
        {
            UserStyle oldUS = _context.UserStyles.Single(d => d.Id == userStyle.Id);
            _context.UserStyles.Remove(oldUS);
            await _context.SaveChangesAsync();
            _context.UserStyles.Add(userStyle);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserStyleExists(userStyle.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Supprime l'utilisateur
        // DELETE: api/Account/5
        [HttpDelete()]
        public async Task<IActionResult> DeleteAspNetUser()
        {
            AspNetUser aspNetUser = GetAspNetUser().Result.Value;
            if (aspNetUser == null)
            {
                return NotFound();
            }
            List<UserLike> userlikes = await _context.UserLikes.Where(ul => ul.Id == aspNetUser.Id || ul.Id1 == aspNetUser.Id).ToListAsync();
            foreach (UserLike like in userlikes)
            {
                _context.UserLikes.Remove(like);
            }
            List<Talk> talks = await _context.Talks.Where(t => t.Id == aspNetUser.Id || t.IdUser2Talk == aspNetUser.Id).ToListAsync();
            foreach (Talk talk in talks)
            {
                _context.Talks.Remove(talk);
            }
            List<Message> messages = await _context.Messages.Where(m => m.Id == aspNetUser.Id).ToListAsync();
            foreach (Message message in messages)
            {
                _context.Messages.Remove(message);
            }
            _context.AspNetUsers.Remove(aspNetUser);
            await _context.SaveChangesAsync();

            // envoyer mail de confirmation
            await _emailSender.SendEmailAsync(
                     aspNetUser.Email,
                     "Your account has been deleted",
                     "Your account has been deleted</br></br> Have a nice day !");

            return Ok();
        }

        private bool AspNetUserExists(string id)
        {
            return _context.AspNetUsers.Any(e => e.Id == id);
        }

        private bool UserStyleExists(string id)
        {
            return _context.UserStyles.Any(e => e.Id == id);
        }
        /*
         *      Auteur : Tim Allemann
         *      11.05.2020
         *      Permet de récupérer les roles de l'utilisateur
         */
        [AllowAnonymous]
        [HttpGet("GetRole/{userId}")]
        public async Task<IEnumerable<string>> GetRole(string userId = "")
        {

            return await _context.AspNetUserRoles
                            .Where(a => a.UserId == userId)
                            .Select(a => a.RoleId)
                            .ToListAsync();
        }
    }
}
