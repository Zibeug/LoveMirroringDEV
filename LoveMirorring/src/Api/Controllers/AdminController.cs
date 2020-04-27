using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc.ViewModels;
using mvc.ViewModels.Admin;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly LoveMirroringContext _context;

        public AdminController(LoveMirroringContext context)
        {
            _context = context;
        }

        [Route("welcom")]
        [HttpGet]
        public async Task<IActionResult> Welcom()
        {
            IndexModel overView = (from u in await _context.AspNetUsers.ToListAsync()
                     select new IndexModel { nbUsers = u.Id.Count() }).FirstOrDefault();

            return new JsonResult(overView);
        }

        [Route("search")]
        [HttpGet]
        public async Task<IActionResult> SearchUser(string username)
        {
            string id = (from u in await _context.AspNetUsers.ToListAsync()
                        where u.UserName.Equals(username) 
                        select u.Id).FirstOrDefault();

            return new JsonResult(id);
        }

        [Route("user")]
        [HttpGet]
        public async Task<IActionResult> GetUser(string id)
        {
            SearchModel user = (from u in await _context.AspNetUsers.ToListAsync()
                     where u.Id.Equals(id)
                     select new SearchModel {
                         UserName = u.UserName, 
                         Email = u.Email, 
                         EmailConfirmed = u.EmailConfirmed, 
                         PhoneNumber = u.PhoneNumber, 
                         PhoneNumberConfirmed = u.PhoneNumberConfirmed, 
                         QuizCompleted = u.QuizCompleted
                     }).FirstOrDefault();

            return new JsonResult(user);
        }
    }
}