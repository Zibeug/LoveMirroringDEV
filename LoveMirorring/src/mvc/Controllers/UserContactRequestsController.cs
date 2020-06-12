using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

using System.Threading.Tasks;
using Newtonsoft.Json;
using Unosquare.Swan;

namespace mvc.Controllers
{
    [Authorize]
    public class UserContactRequestsController : Controller
    {
        private readonly IConfiguration _configuration;

        public UserContactRequestsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: UserContactRequests
        public async Task<IActionResult> Index()
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Récurération des données et convertion des données dans le bon type
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/UserContactRequests");
            List<ContactRequest> userContactRequests = JsonConvert.DeserializeObject<List<ContactRequest>>(content);

            return View(userContactRequests);
        }

        // GET: UserContactRequests/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //// Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            //// Récurération des données et convertion des données dans le bon type
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/UserContactRequests");
            List<ContactRequest> userContactRequests = JsonConvert.DeserializeObject<List<ContactRequest>>(content);
            ContactRequest userContactRequest = userContactRequests.Where(x => x.RequestId == id).SingleOrDefault();

            if (userContactRequest == null)
            {
                return NotFound();
            }
            return View(userContactRequest);
        }

        // GET: RedirectToAnswerCreate/5
        public IActionResult RedirectToAnswerCreate(short id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return Redirect("/AnswerRequests/Create/" + id);
        }

        // GET: UserContactRequests/Create
        public IActionResult CreateAsync()
        {
            return View();
        }

        // POST: UserContactRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequestId,RequestDate,RequestText,Id")] ContactRequest contactRequest)
        {
            try
            {
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

                contactRequest.RequestDate = DateTime.Now;
                contactRequest.Id = user.Id.ToString();

                StringContent httpContent = new StringContent(contactRequest.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(_configuration["URLAPI"] + "api/UserContactRequests", httpContent);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
                }
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException e)
            {
                return Unauthorized();
            }
        }

        // GET: UserContactRequests/Edit/5
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
                string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/UserContactRequests/{id}");

                ContactRequest userContactRequest = JsonConvert.DeserializeObject<ContactRequest>(content);

                if (userContactRequest == null)
                {
                    return NotFound();
                }

                if (userContactRequest.AnswerRequests.Count > 0)
                {
                    return Unauthorized();
                }

                return View(userContactRequest);
            }
            catch (HttpRequestException e)
            {
                return Unauthorized();
            }
        }

        // POST: UserContactRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("RequestId,RequestDate,RequestText,Id")] ContactRequest contactRequest)
        {
            try
            {
                if (id != contactRequest.RequestId)
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
                    StringContent httpContent = new StringContent(contactRequest.ToJson(), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PutAsync(_configuration["URLAPI"] + $"api/UserContactRequests/{id}", httpContent);
                    if (response.StatusCode != HttpStatusCode.NoContent)
                    {
                        return BadRequest();
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(contactRequest);
            }
            catch (HttpRequestException e)
            {
                return Unauthorized();
            }
        }

        // GET: UserContactRequests/Delete/5
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

                //// Récurération des données et convertion des données dans le bon type
                //string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/UserContactRequests/{id}");
                //ContactRequest contactRequest = JsonConvert.DeserializeObject<ContactRequest>(content);

                //// Récurération des données et convertion des données dans le bon type
                string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/UserContactRequests");
                List<ContactRequest> contactRequests = JsonConvert.DeserializeObject<List<ContactRequest>>(content);
                ViewData["ContactId"] = new SelectList(contactRequests, "QuestionId", "QuestionText");
                ContactRequest contactRequest = contactRequests.Where(x => x.RequestId == id).SingleOrDefault();

                if (contactRequest == null)
                {
                    return NotFound();
                }

                return View(contactRequest);
            }
            catch (HttpRequestException e)
            {
                return Unauthorized();
            }
        }

        // POST: UserContactRequests/Delete/5
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
                    HttpResponseMessage response = await client.DeleteAsync(_configuration["URLAPI"] + $"api/UserContactRequests/{id}");

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return BadRequest();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException e)
            {
                return Unauthorized();
            }
        }


    }
}