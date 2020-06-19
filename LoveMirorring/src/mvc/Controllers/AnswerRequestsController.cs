/*
 * Auteur :Paul Gillet
 * Date : 11.06.2020
 * Description : permet de traiter la liste des réponses aux demandes de contact
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Unosquare.Swan;

namespace mvc.Controllers
{
    [Authorize]
    public class AnswerRequestsController : Controller
    {
        private readonly IConfiguration _configuration;

        public AnswerRequestsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: AnswerRequests
        public async Task<IActionResult> Index()
        {
            try
            {
                // Préparation de l'appel à l'API
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Récurération des données et convertion des données dans le bon type
                string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/AnswerRequests");
                List<AnswerRequest> answerRequests = JsonConvert.DeserializeObject<List<AnswerRequest>>(content);

                return View(answerRequests);
            }
            catch (HttpRequestException e)
            {
                return Unauthorized();
            }
        }

        // GET: AnswerRequests/Details/5
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
                string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/AnswerRequests/{id}");
                AnswerRequest answerRequest = JsonConvert.DeserializeObject<AnswerRequest>(content);

                if (answerRequest == null)
                {
                    return NotFound();
                }

                return View(answerRequest);
            }
            catch (HttpRequestException e)
            {
                return Unauthorized();
            }
        }

        // GET: AnswerRequests/Create
        public async Task<IActionResult> CreateAsync(short id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                ViewData["id"] = id;

                // Préparation de l'appel à l'API
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/account/getUserInfo");
                AspNetUser user = JsonConvert.DeserializeObject<AspNetUser>(content);

                if (user == null)
                {
                    return NotFound();
                }

                content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/account/GetRole/{user.Id}");
                List<string> roles = JsonConvert.DeserializeObject<List<string>>(content);

                if (roles == null)
                {
                    return NotFound();
                }

                foreach (var role in roles)
                {
                    if (role.Equals("Administrateur"))
                    {
                        ViewData["layout"] = "~/Views/Shared/_LayoutAdmin.cshtml";
                    }
                    else
                    {
                        ViewData["layout"] = "~/Views/Shared/_Layout.cshtml";
                    }
                }
                return View();
            }
            catch (HttpRequestException e)
            {
                return Unauthorized();
            }
        }

        // POST: AnswerRequests/Create/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AnswerId,AnswerDate,AnswerText,Id,RequestId")] AnswerRequest answerRequest, short id)
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

                string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/account/getUserInfo");
                AspNetUser user = JsonConvert.DeserializeObject<AspNetUser>(content);

                if (user == null)
                {
                    return NotFound();
                }

                answerRequest.AnswerDate = DateTime.Now;
                answerRequest.Id = user.Id.ToString();
                answerRequest.RequestId = id;

                if (ModelState.IsValid)
                {
                    StringContent httpContent = new StringContent(answerRequest.ToJson(), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(_configuration["URLAPI"] + "api/AnswerRequests", httpContent);

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return Unauthorized();
                    }

                    content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/account/GetRole/{user.Id}");
                    List<string> roles = JsonConvert.DeserializeObject<List<string>>(content);

                    if (roles == null)
                    {
                        return NotFound();
                    }

                    foreach (var role in roles)
                    {
                        if (role.Equals("Administrateur"))
                        {
                            return RedirectToAction("Index", "ContactRequests");
                        }
                        else
                        {
                            return RedirectToAction("Index", "UserContactRequests");
                        }
                    }
                }
                return View(answerRequest);
            }
            catch (HttpRequestException e)
            {
                return Unauthorized();
            }
        }
    }
}