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

            ViewData["URLAPI"] = _configuration["URLAPI"];

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
                MultipartFormDataContent form = new MultipartFormDataContent();
                HttpContent content = new StringContent("files");
                form.Add(content, "files");

                foreach (var item in userPictures.Pictures)
                {
                    var stream = item.OpenReadStream();
                    content = new StreamContent(stream);
                    content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "files",
                        FileName = item.FileName
                    };
                    form.Add(content);
                }
                

                HttpResponseMessage response = await client.PostAsync(_configuration["URLAPI"] + "api/Pictures", form);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return BadRequest();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userPictures);
        }

        //GET: Pictures/Delete/5
        public async Task<IActionResult> Delete(short? id)
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
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Pictures/" + id);
            Picture picture = JsonConvert.DeserializeObject<Picture>(content);

            if (picture == null)
            {
                return NotFound();
            }

            ViewData["URLAPI"] = _configuration["URLAPI"];

            return View(picture);
        }

        // POST: Pictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Envoie de la demande du suppression du compte
            HttpResponseMessage content = await client.DeleteAsync(_configuration["URLAPI"] + "api/Pictures/" + id);

            if (content.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest();
            }

        }

        //private bool PictureExists(short id)
        //{
        //    return _context.Pictures.Any(e => e.PictureId == id);
        //}
    }
}
