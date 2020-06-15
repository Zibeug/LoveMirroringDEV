/*
 * Auteur : Gillet Paul
 * Date : 26.05.2020
 * Description : Contrôleur pour afficher et traiter les questions
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class QuestionsController : Controller
    {
        private readonly IConfiguration _configuration;

        public QuestionsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: Questions
        public async Task<IActionResult> Index()
        {
            try
            {
                // Préparation de l'appel à l'API
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Récurération des données et convertion des données dans le bon type
                string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Questions");
                List<Question> questions = JsonConvert.DeserializeObject<List<Question>>(content);

                return View(questions);
            }
            catch (HttpRequestException)
            {
                return Unauthorized();
            }
            
        }

        // GET: Questions/Details/5
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
                string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Questions/{id}");
                Question question = JsonConvert.DeserializeObject<Question>(content);

                if (question == null)
                {
                    return NotFound();
                }

                return View(question);
            }
            catch (HttpRequestException)
            {
                return Unauthorized();
            }

            
        }

        // GET: Questions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuestionId,QuestionText")] Question question)
        {
            try
            {
                // Préparation de l'appel à l'API
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                if (ModelState.IsValid)
                {
                    StringContent httpContent = new StringContent(question.ToJson(), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(_configuration["URLAPI"] + "api/Questions", httpContent);

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return Unauthorized();
                    }

                    return RedirectToAction(nameof(Index));
                }
                return View(question);
            }
            catch (HttpRequestException)
            {
                return Unauthorized();
            }
            
        }

        // GET: Questions/Edit/5
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
                string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Questions/{id}");

                Question question = JsonConvert.DeserializeObject<Question>(content);

                if (question == null)
                {
                    return NotFound();
                }
                return View(question);
            }
            catch (HttpRequestException)
            {
                return Unauthorized();
            }
            
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("QuestionId,QuestionText")] Question question)
        {
            try
            {
                if (id != question.QuestionId)
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
                    StringContent httpContent = new StringContent(question.ToJson(), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PutAsync(_configuration["URLAPI"] + $"api/Questions/{id}", httpContent);
                    if (response.StatusCode != HttpStatusCode.NoContent)
                    {
                        return BadRequest();
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(question);
            }
            catch (HttpRequestException)
            {
                return Unauthorized();
            }
            
        }

        // GET: Questions/Delete/5
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
                string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Questions/{id}");
                Question question = JsonConvert.DeserializeObject<Question>(content);

                if (question == null)
                {
                    return NotFound();
                }

                return View(question);
            }
            catch (HttpRequestException)
            {
                return Unauthorized();
            }
            
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short? id)
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

                if (ModelState.IsValid)
                {
                    HttpResponseMessage response = await client.DeleteAsync(_configuration["URLAPI"] + $"api/Questions/{id}");

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