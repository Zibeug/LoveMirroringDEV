using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using System;
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
    public class UserContactRequestsController : Controller
    {
        private readonly IConfiguration _configuration;

        public object JsonConvert { get; private set; }

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

            if (ModelState.IsValid)
            {
                StringContent httpContent = new StringContent(contactRequest.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(_configuration["URLAPI"] + "api/UserContactRequests", httpContent);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(contactRequest);
        }

        //// GET: UserContactRequests/Edit/5
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
        //    ViewData["Id"] = new SelectList(_context.AspNetUsers, "Id", "Id", contactRequest.Id);
        //    return View(contactRequest);
        //}

        //// POST: UserContactRequests/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(short id, [Bind("RequestId,RequestDate,RequestText,Id")] ContactRequest contactRequest)
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
        //    ViewData["Id"] = new SelectList(_context.AspNetUsers, "Id", "Id", contactRequest.Id);
        //    return View(contactRequest);
        //}

        //// GET: UserContactRequests/Delete/5
        //public async Task<IActionResult> Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var contactRequest = await _context.ContactRequests
        //        .Include(c => c.IdNavigation)
        //        .FirstOrDefaultAsync(m => m.RequestId == id);
        //    if (contactRequest == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(contactRequest);
        //}

        //// POST: UserContactRequests/Delete/5
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