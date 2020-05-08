using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Api.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Api.Controllers
{
    [Authorize(Policy = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly LoveMirroringContext _context;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<AspNetRole> _roleManager;
        private readonly UserManager<AspNetUser> _userManager;


        public AdminController(LoveMirroringContext context, IEmailSender emailSender, RoleManager<AspNetRole> roleManager, UserManager<AspNetUser> userManager)
        {
            _context = context;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [Route("Welcome")]
        [HttpGet]
        public async Task<IActionResult> Welcom()
        {
            try
            {
                int accounts = await _context.AspNetUsers.CountAsync();
                IndexModel overView = new IndexModel
                {
                    nbUsers = accounts
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
                             where u.NormalizedUserName.Equals(UserName)
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
        [HttpPut("{id}")]
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
            await _roleManager.CreateAsync(new AspNetRole { Name = role.Name, NormalizedName = role.Name.ToUpper() });

            return NoContent();
        }

        [Route("UpdateUserRole")]
        [HttpPost]
        public async Task<IActionResult> UpdateUserRole(UpdateUserRoleModel vm)
        {
            if (vm.UserEmail != null && vm.Role != null)
            {

                AspNetUser user = await _userManager.FindByEmailAsync(vm.UserEmail);
                AspNetRole role = await _roleManager.FindByNameAsync(vm.Role);

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
    }
}