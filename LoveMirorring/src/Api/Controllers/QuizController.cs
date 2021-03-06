﻿/*
 * Auteur : Sébastien Berger
 * Date : 06.05.2020
 * Détail : Contrôleur pour pouvoir gérer le Quiz afin d'établir un profil pour l'utilisateur
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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : Controller
    {
        private readonly LoveMirroringContext _context;
        private IConfiguration Configuration { get; set; }

        public QuizController(LoveMirroringContext context, IConfiguration configuration)
        {
            Configuration = configuration;
            _context = context;
        }

        // Permet de retourner les questions contenu dans la base de données
        // GET : api/Quiz
        [HttpGet]
        [Authorize]
        public IActionResult GetQuestion()
        {
            List<Question> questions = _context.Questions.Include(q =>q.Answers).ToList();

            return new JsonResult(questions);
        }

        // Permet de récupérer l'ensemble des réponses dans la base de données
        // GET : api/Quiz/answer
        [Route("answer")]
        [HttpGet]
        [Authorize]
        public IActionResult GetAnswer()
        {
            List<Answer> responses = _context.Answers.ToList();
            return new JsonResult(responses);
        }
        
        // Permet de vérifier si le quiz a déjà été rempli
        // GET : api/Quiz/checkQuiz
        [Route("checkQuiz")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CheckQuiz()
        {
            AspNetUser user = null;
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Récurération des données et convertion des données dans le bon type
            string content = await client.GetStringAsync(Configuration["URLAPI"] + "api/Account/getUserInfo");
            user = JsonConvert.DeserializeObject<AspNetUser>(content);
            if (user.QuizCompleted)
            {
                return new JsonResult("success");
            }
            else
            {
                return new JsonResult("error");
            }
           
        }

        //Permet d'envoyer le quiz une fois qu'il a été rempli
        // POST : api/QuizSubmit
        [Route("QuizSubmit")]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> QuizSubmit(int[] answer)
        {
            
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            AspNetUser user = null;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            //Récupération de l'utilisateur courant
            string userString = await client.GetStringAsync(Configuration["URLAPI"] + "api/Account/getUserInfo");
            user = JsonConvert.DeserializeObject<AspNetUser>(userString);

            int query = (from item in answer
                        group item by item into g
                        orderby g.Count() descending
                        select g.Key).First();

            UserProfil userProfil = new UserProfil();
            userProfil.ProfilId = (short)query;
            userProfil.Id = user.Id;

            user.QuizCompleted = true;

            try
            {
                _context.AspNetUsers.Update(user);

                _context.UserProfils.Add(userProfil);
                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}