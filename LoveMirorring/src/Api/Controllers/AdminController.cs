using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Api.ViewModels.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly LoveMirroringContext _context;
        private readonly IEmailSender _emailSender;

        public AdminController(LoveMirroringContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }


        [Route("welcom")]
        [HttpGet]
        public async Task<IActionResult> Welcom()
        {
            IndexModel overView = (from u in await _context.AspNetUsers.ToListAsync()
                                   select new IndexModel { nbUsers = u.Email.Count() }).FirstOrDefault();

            return new JsonResult(overView);
        }

        [Route("search/{username}")]
        [HttpGet]
        public async Task<IActionResult> SearchUser(string username)
        {
            string id = (from u in await _context.AspNetUsers.ToListAsync()
                         where u.UserName.Equals(username)
                         select u.Id).FirstOrDefault();

            return new JsonResult(id);
        }

        [Route("user/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetUser(string id)
        {
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

            return new JsonResult(user);
        }

        [Route("details/{id}")]
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            AspNetUser user = await _context.AspNetUsers.FindAsync(id);

            return new JsonResult(user);
        }

        [Route("edit")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(AspNetUser user)
        {
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id))
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

        private bool UserExists(string id)
        {
            return _context.AspNetUsers.Any(e => e.Id.Equals(id));
        }

        [Route("delete")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            AspNetUser user = await _context.AspNetUsers.FindAsync(id);
            _context.AspNetUsers.Remove(user);
            await _context.SaveChangesAsync();

            await _emailSender.SendEmailAsync(
         user.Email,
         "Your account has been deleted by the Administrator",
         "Your account has been deleted</br></br> Have a nice day !");

            return Ok();
        }

    }
}