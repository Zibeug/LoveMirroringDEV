/*
 *      Auteur : Hans Morsch
 *      11.05.2020
 *      Contrôleur Api pour l'admin
 *      Permet de gérer les utilisateurs et rôles
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Api.Models;
using Api.ViewModels.Admin;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly LoveMirroringContext _context;
        private readonly IEmailSender _emailSender;
        private IConfiguration Configuration { get; set; }

        public AdminController(LoveMirroringContext context, IEmailSender emailSender, IConfiguration configuration)
        {
            _context = context;
            _emailSender = emailSender;
            Configuration = configuration;
        }

        [Route("Welcome")]
        [HttpGet]
        public async Task<IActionResult> Welcom()
        {
            try
            {
                int accounts = await _context.AspNetUsers.CountAsync();
                List<UserSubscription> userSubscriptionsMonthly = await _context.UserSubscriptions.Where(d => d.UserSubscriptionsId == 1).ToListAsync();
                List<UserSubscription> userSubscriptionsAnnualy = await _context.UserSubscriptions.Where(d => d.UserSubscriptionsId == 2).ToListAsync();

                int nbConnexion = _context.UserTraces.Count();

                AspNetUser user = null;
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Récurération des données et convertion des données dans le bon type
                string content = await client.GetStringAsync(Configuration["URLAPI"] + "api/Account/getUserInfo");
                user = JsonConvert.DeserializeObject<AspNetUser>(content);

                decimal earningMonthly = 0;
                decimal earningAnnualy = 0;

                foreach(UserSubscription u in userSubscriptionsMonthly)
                {
                    earningMonthly += u.UserSubscriptionsAmount;
                }

                foreach(UserSubscription u in userSubscriptionsAnnualy)
                {
                    earningAnnualy = u.UserSubscriptionsAmount;
                }

                IndexModel overView = new IndexModel
                {
                    nbUsers = accounts,
                    earningsMonthly = earningMonthly,
                    earningsAnnualy = earningAnnualy,
                    nbConnexion = nbConnexion
                };

                return new JsonResult(overView);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [Route("Search/{username}")]
        [HttpGet]
        public async Task<IActionResult> SearchUser(string username)
        {
            try
            {
                if (username == null)
                {
                    throw new ArgumentNullException();
                }

                string UserName = username.ToUpper();

                string id = (from u in await _context.AspNetUsers.ToListAsync()
                             where u.NormalizedUserName.Contains(UserName)
                             select u.Id).FirstOrDefault();

                if (id == null) 
                {
                    return new JsonResult(null);
                }

                return new JsonResult(id);
            }
            catch (ArgumentNullException)
            {
                return NotFound("Argument is null");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /*
         * Auteur : Sébastien Berger 
         * Date : 18.05.2020
         * Description : permet de récupérer l'ensembles des utilisateurs pour les afficher dans l'interface administrateur
         */
        [Route("GetAllUsers")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                List<AspNetUser> users = await _context.AspNetUsers.ToListAsync();

                if (users == null)
                {
                    return NotFound();
                }
                else
                {
                    return new JsonResult(users);
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            
        }

        /*
         * Auteur : Sébastien Berger 
         * Date : 18.05.2020
         * Description : permet de récupérer la liste des utilisateurs qui ont été bannis et les afficher dans l'interface administrateur
         */
        [Route("GetAllBan")]
        [HttpGet]
        public async Task<IActionResult> GetAllBan()
        {
            try
            {
                List<AspNetUser> users = await _context.AspNetUsers.Where(d => d.LockoutEnd != null).ToListAsync();

                return new JsonResult(users);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /*
         * Auteur : Sébastien Berger 
         * Date : 18.05.2020
         * Description : permet de mettre à jour un utilisateur s'il est de nouveau autorisé à utiliser l'application
         */
        [Route("UnBan")]
        [HttpPut]
        public async Task<IActionResult> UnBan([FromBody]string username)
        {
            try
            {
                if (username == null)
                {
                    return NotFound();
                }

                AspNetUser user = _context.AspNetUsers.Where(d => d.UserName.Equals(username)).Single();

                if (user == null)
                {
                    return NotFound();
                }

                user.LockoutEnd = null;

                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /*
         * Auteur : Sébastien Berger 
         * Date : 18.05.2020
         * Description : Permet de récupérer les utilisateurs qui ont répondu au Quizz
         */
        [Route("GetAllQuiz")]
        [HttpGet]
        public async Task<IActionResult> GetAllQuiz()
        {
            try
            {
                List<AspNetUser> users = await _context.AspNetUsers.Where(d => d.QuizCompleted == true).ToListAsync();

                return new JsonResult(users);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /*
         * Auteur : Sébastien Berger 
         * Date : 18.05.2020
         * Description : Permet de supprimer la relations qui lie un profil à un utilisateur et remet la valeur du Quiz à zéro pour l'utilisateur.
         */
        [Route("ResetQuiz/{id}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> ResetQuiz(string id)
        {
            try
            {
                AspNetUser user = _context.AspNetUsers.Single(d => d.UserName == id);

                if(user == null)
                {
                    return NotFound();
                }

                UserProfil userProfil = _context.UserProfils.Where(d => d.Id == user.Id).SingleOrDefault();
                user.QuizCompleted = false;

                _context.UserProfils.Remove(userProfil);
                _context.AspNetUsers.Update(user);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch(Exception)
            {
                return StatusCode(500);
            }
        }

        [Route("UserToValidate")]
        [HttpGet]
        public async Task<IActionResult> UserToValidate()
        {
            try
            {
                List<AspNetUser> users = await _context.AspNetUsers.Where(d => d.EmailConfirmed == false || d.PhoneNumberConfirmed == false).ToListAsync();
                return new JsonResult(users);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Route("GetUser/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetUser(string id)
        {
            try
            {
                if (id == null)
                {
                    throw new ArgumentNullException();
                }

                SearchModel user = (from u in await _context.AspNetUsers.ToListAsync()
                                    where u.Id.Equals(id)
                                    select new SearchModel
                                    {
                                        Id = u.Id,
                                        UserName = u.UserName,
                                        Email = u.Email,
                                        EmailConfirmed = u.EmailConfirmed,
                                        PhoneNumber = u.PhoneNumber,
                                        PhoneNumberConfirmed = u.PhoneNumberConfirmed,
                                        QuizCompleted = u.QuizCompleted
                                    }).FirstOrDefault();

                if(user == null) 
                {
                    return new JsonResult(null);
                }

                return new JsonResult(user);
            }
            catch (ArgumentNullException)
            {
                return NotFound("Argument is null");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [Route("Details/{id}")]
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                if (id == null)
                {
                    throw new ArgumentNullException();
                }

                AspNetUser user = await _context.AspNetUsers.FindAsync(id);

                if (user == null)
                {
                    return new JsonResult(null);
                }

                return new JsonResult(user);
            }
            catch (ArgumentNullException)
            {
                return NotFound("Argument is null");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [Route("Edit")]
        [HttpPut]
        public async Task<IActionResult> Edit(AspNetUser user)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return NotFound("Argument is null");
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id))
                {
                    return new JsonResult(null);
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        private bool UserExists(string id)
        {
            return _context.AspNetUsers.Any(e => e.Id.Equals(id));
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                if (id == null)
                {
                    throw new ArgumentNullException();
                }

                AspNetUser user = await _context.AspNetUsers.FindAsync(id);

                if (user == null)
                {
                    return new JsonResult(null);
                }

                _context.AspNetUsers.Remove(user);
                await _context.SaveChangesAsync();

                await _emailSender.SendEmailAsync(
             user.Email,
             "Your account has been deleted by the Administrator",
             "Your account has been deleted</br></br> Have a nice day !");

                return Ok();

            }
            catch (ArgumentNullException)
            {
                return NotFound("Argument is null");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }

        [Route("Roles")]
        [HttpGet]
        public async Task<IActionResult> UserRole()
        {
            List<AspNetRole> roles = await _context.AspNetRoles.ToListAsync();
            List<AspNetUser> users = await _context.AspNetUsers.ToListAsync();
            List<AspNetUserRole> userroles = _context.AspNetUserRoles.ToList();

            IEnumerable<UsersModel> convertedUsers = users.Select(u => new UsersModel
            {
                Email = u.Email,
                Roles = roles
                    .Where(r => userroles.Any(ur => ur.UserId == u.Id && ur.RoleId == r.Id))
                    .Select(r => new AspNetRole
                    {
                        NormalizedName = r.NormalizedName
                    })

            });

            return new JsonResult(new RolesModel 
            { 
                Roles = roles.Select(r => r.NormalizedName),
                Users = convertedUsers
            
            });
        }

        [Route("CreateRole")]
        [HttpPost]
        public async Task<IActionResult> CreateRole(AspNetRole role)
        {
            AspNetRole exist = await _context.AspNetRoles.Where(r => r.NormalizedName == role.Name.ToUpper()).FirstOrDefaultAsync();

            if (exist == null)
            {
                AspNetRole newRole = new AspNetRole { Id = role.Name, Name = role.Name, NormalizedName = role.Name.ToUpper() };
                await _context.AspNetRoles.AddAsync(newRole);
                await _context.SaveChangesAsync();
                
                return NoContent();
            }
            else
            {
                return BadRequest();
            }

        }

        [Route("UpdateUserRole")]
        [HttpPost]
        public async Task<IActionResult> UpdateUserRole(UpdateUserRoleModel vm)
        {
            if (vm.UserEmail != null && vm.Role != null)
            {
                AspNetUser user = await _context.AspNetUsers.Where(u => u.Email == vm.UserEmail).FirstOrDefaultAsync();
                AspNetRole role = await _context.AspNetRoles.Where(r => r.NormalizedName == vm.Role).FirstOrDefaultAsync();
                AspNetUserRole userRole = _context.AspNetUserRoles.Where(r => r.RoleId.Equals(role.Id)).Where(u => u.UserId.Equals(user.Id)).FirstOrDefault();

                if (vm.DeleteRole)
                {
                    _context.AspNetUserRoles.Remove(userRole);
                }
                else
                {
                    _context.AspNetUserRoles.Add(new AspNetUserRole { RoleId = role.Id, UserId = user.Id });
                }

                await _context.SaveChangesAsync();
            }
            return NoContent();
        }

        private string GeneratePassword()
        {
            int length = 10;

            bool nonAlphanumeric = true;
            bool digit = true;
            bool lowercase = true;
            bool uppercase = true;

            StringBuilder password = new StringBuilder();
            Random random = new Random();

            while (password.Length < length)
            {
                char c = (char)random.Next(32, 126);

                password.Append(c);

                if (char.IsDigit(c))
                    digit = false;
                else if (char.IsLower(c))
                    lowercase = false;
                else if (char.IsUpper(c))
                    uppercase = false;
                else if (!char.IsLetterOrDigit(c))
                    nonAlphanumeric = false;
            }

            if (nonAlphanumeric)
                password.Append((char)random.Next(33, 48));
            if (digit)
                password.Append((char)random.Next(48, 58));
            if (lowercase)
                password.Append((char)random.Next(97, 123));
            if (uppercase)
                password.Append((char)random.Next(65, 91));

            return password.ToString();
        }

        [Route("GiveNewPassword/{id}")]
        [HttpPut]
        public async Task<IActionResult> GiveNewPassword(string id)
        {
            if (id != null)
            {
                string newpassword = GeneratePassword();
                var hashed = new PasswordHasher<AspNetUser>();
                AspNetUser user = await _context.AspNetUsers.Where(u => u.Id == id).FirstOrDefaultAsync();
                user.PasswordHash = hashed.HashPassword(user, newpassword);
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                await _emailSender.SendEmailAsync(
                    user.Email,
                    "Your password has been changed from the Administrator.",
                    $"Your new password is -->{newpassword}<--." + "</br> Have a nice day !");

                return Ok();
            }
            return null;
        }

    }
}