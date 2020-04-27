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

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : Controller
    {
        private readonly LoveMirroringContext _context;
        private IConfiguration Configuration { get; set; }

        public QuizController(LoveMirroringContext context)
        {

            _context = context;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetQuestion()
        {
            List<Question> questions = _context.Questions.Include(q =>q.Answers).ToList();

            return new JsonResult(questions);
        }

        [Route("answer")]
        [HttpGet]
        [Authorize]
        public IActionResult GetAnswer()
        {
            List<Answer> responses = _context.Answers.ToList();
            return new JsonResult(responses);
        }

        //public async Task<IActionResult> ConfirmQuiz(string userId)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if(user != null)
        //    {
        //        //user.QuizCompleted = true;
        //    }
            
        //    return View();
        //}

        [Route("QuizSubmit")]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> QuizSubmit(int[] answer)
        {
            AspNetUser user = null;
            foreach (var claim in User.Claims)
            {
                if (user == null)
                {
                    user = _context.AspNetUsers.Find(claim.Value);
                }
            }

            int query = (from item in answer
                        group item by item into g
                        orderby g.Count() descending
                        select g.Key).First();

            UserProfil userProfil = new UserProfil();
            userProfil.ProfilId = (short)query;
            userProfil.Id = user.Id;

            user.QuizCompleted = true;

            _context.UserProfils.Add(userProfil);
            _context.SaveChanges();

            return Ok();
        }
    }
}