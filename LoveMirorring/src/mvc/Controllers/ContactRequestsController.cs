/*
 * Auteur :Paul Gillet
 * Date : 27.05.2020
 * Description : permet de récupérer la liste des demandes de contact et les afficher dans l'interface administrateur
 */

using Microsoft.AspNetCore.Authentication;
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

        //// GET: ContactRequests/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: ContactRequests/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("RequestId,RequestDate,RequestText,UserId")] ContactRequest contactRequest)
        //{
        //    // Préparation de l'appel à l'API
        //    string accessToken = await HttpContext.GetTokenAsync("access_token");
        //    HttpClient client = new HttpClient();
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        //    if (ModelState.IsValid)
        //    {
        //        StringContent httpContent = new StringContent(contactRequest.ToJson(), Encoding.UTF8, "application/json");
        //        HttpResponseMessage response = await client.PostAsync(_configuration["URLAPI"] + "api/ContactRequests", httpContent);

        //        if (response.StatusCode == HttpStatusCode.Unauthorized)
        //        {
        //            return Unauthorized();
        //        }

        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(contactRequest);
        //}

        //// GET: ContactRequests/Edit/5
        //public async Task<IActionResult> Edit(short? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var contactRequest = await _context.ContactRequests.FindAsync(id);
        //    if (contactRequest == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", contactRequest.UserId);
        //    return View(contactRequest);
        //}

        //// POST: ContactRequests/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(short id, [Bind("RequestId,RequestDate,RequestText,UserId")] ContactRequest contactRequest)
        //{
        //    if (id != contactRequest.RequestId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(contactRequest);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ContactRequestExists(contactRequest.RequestId))
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
        //    ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", contactRequest.UserId);
        //    return View(contactRequest);
        //}

        //// GET: ContactRequests/Delete/5
        //public async Task<IActionResult> Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var contactRequest = await _context.ContactRequests
        //        .Include(c => c.User)
        //        .FirstOrDefaultAsync(m => m.RequestId == id);
        //    if (contactRequest == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(contactRequest);
        //}

        //// POST: ContactRequests/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(short id)
        //{
        //    var contactRequest = await _context.ContactRequests.FindAsync(id);
        //    _context.ContactRequests.Remove(contactRequest);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ContactRequestExists(short id)
        //{
        //    return _context.ContactRequests.Any(e => e.RequestId == id);
        //}
    }
}