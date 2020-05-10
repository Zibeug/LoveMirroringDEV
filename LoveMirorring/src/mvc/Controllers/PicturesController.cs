using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using mvc.ViewModels;
using Newtonsoft.Json;
using Unosquare.Swan;

namespace mvc.Controllers
{
    public class PicturesController : Controller
    {
        private readonly IConfiguration _configuration;

        public PicturesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: Pictures
        public async Task<IActionResult> Index()
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

            content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Pictures");
            List<Picture> pictures = JsonConvert.DeserializeObject<List<Picture>>(content);

            return View(pictures);
        }

        // GET: Pictures/Details/5
        //public async Task<IActionResult> Details(short? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var picture = await _context.Pictures
        //        .Include(p => p.IdNavigation)
        //        .FirstOrDefaultAsync(m => m.PictureId == id);
        //    if (picture == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(picture);
        //}

        // GET: Pictures/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pictures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Pictures")] UserPictures userPictures)
        {
            if (ModelState.IsValid)
            {
                // Préparation de l'appel à l'API
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Préparation de la requête update à l'API
                StringContent httpContent = new StringContent(userPictures.Pictures.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(_configuration["URLAPI"] + "api/Pictures", httpContent);
                if (response.StatusCode != HttpStatusCode.Created)
                {
                    return BadRequest();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userPictures);
        }

        // GET: Pictures/Edit/5
        //public async Task<IActionResult> Edit(short? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var picture = await _context.Pictures.FindAsync(id);
        //    if (picture == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["Id"] = new SelectList(_context.AspNetUsers, "Id", "Id", picture.Id);
        //    return View(picture);
        //}

        // POST: Pictures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(short id, [Bind("PictureId,Id,PictureView")] Picture picture)
        //{
        //    if (id != picture.PictureId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(picture);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!PictureExists(picture.PictureId))
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
        //    ViewData["Id"] = new SelectList(_context.AspNetUsers, "Id", "Id", picture.Id);
        //    return View(picture);
        //}

        // GET: Pictures/Delete/5
        //public async Task<IActionResult> Delete(short? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var picture = await _context.Pictures
        //        .Include(p => p.IdNavigation)
        //        .FirstOrDefaultAsync(m => m.PictureId == id);
        //    if (picture == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(picture);
        //}

        // POST: Pictures/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(short id)
        //{
        //    var picture = await _context.Pictures.FindAsync(id);
        //    _context.Pictures.Remove(picture);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool PictureExists(short id)
        //{
        //    return _context.Pictures.Any(e => e.PictureId == id);
        //}
    }
}
