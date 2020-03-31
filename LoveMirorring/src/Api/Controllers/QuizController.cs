using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : Controller
    {
        private readonly LoveMirroringContext _context;

        public QuizController(LoveMirroringContext context)
        {
            
            _context = context;
        }

        [HttpGet]
        public IActionResult GetQuestion()
        {
            List<Question> questions = _context.Questions.ToList();
            return new JsonResult(questions);
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
    }
}