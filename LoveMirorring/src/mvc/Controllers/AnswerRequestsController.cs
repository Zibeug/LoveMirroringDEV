/*
 * Auteur :Paul Gillet
 * Date : 11.06.2020
 * Description : permet de traiter la liste des réponses aux demandes de contact
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using Newtonsoft.Json;
using Unosquare.Swan;

namespace mvc.Controllers
{
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

        //// GET: AnswerRequests/Edit/5
        //public async Task<IActionResult> Edit(short? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var answerRequest = await _context.AnswerRequests.FindAsync(id);
        //    if (answerRequest == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["Id"] = new SelectList(_context.AspNetUsers, "Id", "Id", answerRequest.Id);
        //    ViewData["RequestId"] = new SelectList(_context.ContactRequests, "RequestId", "Id", answerRequest.RequestId);
        //    return View(answerRequest);
        //}

        //// POST: AnswerRequests/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(short id, [Bind("AnswerId,AnswerDate,AnswerText,Id,RequestId")] AnswerRequest answerRequest)
        //{
        //    if (id != answerRequest.AnswerId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(answerRequest);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!AnswerRequestExists(answerRequest.AnswerId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["Id"] = new SelectList(_context.AspNetUsers, "Id", "Id", answerRequest.Id);
        //    ViewData["RequestId"] = new SelectList(_context.ContactRequests, "RequestId", "Id", answerRequest.RequestId);
        //    return View(answerRequest);
        //}

        //// GET: AnswerRequests/Delete/5
        //public async Task<IActionResult> Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var answerRequest = await _context.AnswerRequests
        //        .Include(a => a.IdNavigation)
        //        .Include(a => a.Request)
        //        .FirstOrDefaultAsync(m => m.AnswerId == id);
        //    if (answerRequest == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(answerRequest);
        //}

        //// POST: AnswerRequests/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(short id)
        //{
        //    var answerRequest = await _context.AnswerRequests.FindAsync(id);
        //    _context.AnswerRequests.Remove(answerRequest);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool AnswerRequestExists(short id)
        //{
        //    return _context.AnswerRequests.Any(e => e.AnswerId == id);
        //}
    }
}
