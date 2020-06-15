/*
 * Auteur :Paul Gillet
 * Date : 27.05.2020
 * Description : permet de récupérer la liste des demandes de contact et les afficher dans l'interface administrateur
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Unosquare.Swan;

namespace mvc.Controllers
{
    [Authorize(Policy = "Administrateur")]
    public class ContactRequestsController : Controller
    {
        private readonly IConfiguration _configuration;

        public ContactRequestsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: ContactRequests
        public async Task<IActionResult> Index()
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Récurération des données et convertion des données dans le bon type
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/ContactRequests");
            List<ContactRequest> contactRequests = JsonConvert.DeserializeObject<List<ContactRequest>>(content);

            return View(contactRequests);
        }

        // GET: ContactRequests/Details/5
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
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/ContactRequests");
            List<ContactRequest> contactRequests = JsonConvert.DeserializeObject<List<ContactRequest>>(content);
            ViewData["ContactId"] = new SelectList(contactRequests, "QuestionId", "QuestionText");
            ContactRequest contactRequest = contactRequests.Where(x => x.RequestId == id).SingleOrDefault();

            if (contactRequests == null)
            {
                return NotFound();
            }
            return View(contactRequest);
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

        // GET: ContactRequests/Edit/5
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
                string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/ContactRequests/{id}");

                ContactRequest contactRequest = JsonConvert.DeserializeObject<ContactRequest>(content);

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

        // POST: ContactRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("RequestId,RequestDate,RequestText,UserId")] ContactRequest contactRequest)
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
                    HttpResponseMessage response = await client.PutAsync(_configuration["URLAPI"] + $"api/ContactRequests/{id}", httpContent);
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

        // GET: ContactRequests/Delete/5
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
                string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/ContactRequests/{id}");
                ContactRequest contactRequest = JsonConvert.DeserializeObject<ContactRequest>(content);

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

        // POST: ContactRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
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
                    HttpResponseMessage response = await client.DeleteAsync(_configuration["URLAPI"] + $"api/ContactRequests/{id}");

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