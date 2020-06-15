/*
 * Auteur : Gillet Paul
 * Date : 26.05.2020
 * Description : Contrôleur pour afficher et traiter les réponses
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Unosquare.Swan;

namespace mvc.Controllers
{
    [Authorize(Policy = "Administrateur")]
    public class AnswersController : Controller
    {
        private readonly IConfiguration _configuration;

        public AnswersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: Answers
        public async Task<IActionResult> Index()
        {
            try
            {
                // Préparation de l'appel à l'API
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Récurération des données et convertion des données dans le bon type
                string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Answers");
                List<Answer> answers = JsonConvert.DeserializeObject<List<Answer>>(content);
                return View(answers);
            }
            catch (HttpRequestException)
            {
                return Unauthorized();
            }
            
        }

        // GET: Answers/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Préparation de l'appel à l'API
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Récurération des données et convertion des données dans le bon type
                string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Answers/{id}");
                Answer answer = JsonConvert.DeserializeObject<Answer>(content);

                if (answer == null)
                {
                    return NotFound();
                }

                return View(answer);
            }
            catch (HttpRequestException)
            {
                return Unauthorized();
            }
            
        }

        // GET: Answers/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                // Préparation de l'appel à l'API
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Récurération des données et convertion des données dans le bon type
                string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Questions");
                List<Question> questions = JsonConvert.DeserializeObject<List<Question>>(content);
                ViewData["QuestionId"] = new SelectList(questions, "QuestionId", "QuestionText");

                content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Profils");
                List<Profil> profils = JsonConvert.DeserializeObject<List<Profil>>(content);
                ViewData["ProfilId"] = new SelectList(profils, "ProfilId", "ProfilDescription");

                return View();
            }
            catch (HttpRequestException)
            {
                return Unauthorized();
            }
            
        }

        // POST: Answers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AnswerId,ProfilId,QuestionId,AnswerText,AnswerValue")] Answer answer)
        {
            try
            {
                // Préparation de l'appel à l'API
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                if (ModelState.IsValid)
                {
                    StringContent httpContent = new StringContent(answer.ToJson(), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(_configuration["URLAPI"] + "api/Answers", httpContent);

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return Unauthorized();
                    }

                    return RedirectToAction(nameof(Index));
                }
                return View(answer);
            }
            catch (HttpRequestException)
            {
                return Unauthorized();
            }
            
        }

        // GET: Answers/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Préparation de l'appel à l'API
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Récurération des données et convertion des données dans le bon type
                string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Answers/{id}");

                Answer answer = JsonConvert.DeserializeObject<Answer>(content);

                // Récurération des données et convertion des données dans le bon type
                content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Questions");
                List<Question> questions = JsonConvert.DeserializeObject<List<Question>>(content);
                ViewData["QuestionId"] = new SelectList(questions, "QuestionId", "QuestionText");

                content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Profils");
                List<Profil> profils = JsonConvert.DeserializeObject<List<Profil>>(content);
                ViewData["ProfilId"] = new SelectList(profils, "ProfilId", "ProfilDescription");

                if (answer == null)
                {
                    return NotFound();
                }
                return View(answer);
            }
            catch (HttpRequestException)
            {
                return Unauthorized();
            }
            
        }

        // POST: Answers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("AnswerId,ProfilId,QuestionId,AnswerText,AnswerValue")] Answer answer)
        {
            try
            {
                if (id != answer.AnswerId)
                {
                    return NotFound();
                }

                // Préparation de l'appel à l'API
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                if (ModelState.IsValid)
                {
                    // Préparation de la requête update à l'API
                    StringContent httpContent = new StringContent(answer.ToJson(), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PutAsync(_configuration["URLAPI"] + $"api/Answers/{id}", httpContent);
                    if (response.StatusCode != HttpStatusCode.NoContent)
                    {
                        return BadRequest();
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(answer);
            }
            catch (HttpRequestException)
            {
                return Unauthorized();
            }
            
        }

        // GET: Answers/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Préparation de l'appel à l'API
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Récurération des données et convertion des données dans le bon type
                string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Answers/{id}");
                Answer answer = JsonConvert.DeserializeObject<Answer>(content);

                if (answer == null)
                {
                    return NotFound();
                }

                return View(answer);
            }
            catch (HttpRequestException)
            {
                return Unauthorized();
            }
            
        }

        // POST: Answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            try
            {
                // Préparation de l'appel à l'API
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                if (ModelState.IsValid)
                {
                    HttpResponseMessage response = await client.DeleteAsync(_configuration["URLAPI"] + $"api/Answers/{id}");

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return BadRequest();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException)
            {
                return Unauthorized();
            }
            
        }
    }
}