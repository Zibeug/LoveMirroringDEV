/*
 * Auteur : Sébastien Berger 
 * Date : 30.05.2020
 * Description : Permet de gérer les publicités côté Administrateur.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using mvc.Models;
using mvc.ViewModels;
using Newtonsoft.Json;
using Unosquare.Swan;

namespace mvc.Controllers
{
    [Authorize(Policy = "Administrateur")]
    public class AdsController : Controller
    {
        private IConfiguration Configuration { get; set; }
        public IStringLocalizer<MatchingController> _localizer;
        public AdsController(IConfiguration configuration, IStringLocalizer<MatchingController> localizer)
        {
            Configuration = configuration;
            _localizer = localizer;
        }

        // GET: Ads
        public async Task<IActionResult> Index()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri(Configuration["URLAPI"] + "api/Ads");
            var ad = await client.GetStringAsync(client.BaseAddress);
            IEnumerable<Ad> ads = JsonConvert.DeserializeObject<IEnumerable<Ad>>(ad);
            return View(ads);
        }

        // GET: Ads/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri(Configuration["URLAPI"] + "api/Ads/" + id);

            var ad = await client.GetStringAsync(client.BaseAddress);
            Ad ads = JsonConvert.DeserializeObject<Ad>(ad);
            if (ads == null)
            {
                return NotFound();
            }

            ViewData["URLAPI"] = Configuration["URLAPI"];

            return View(ads);
        }

        // GET: Ads/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titre,Description,file,Link")] AdInput ad)
        {
            if (ModelState.IsValid)
            {
                string accessToken = await HttpContext.GetTokenAsync("access_token");

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.BaseAddress = new Uri(Configuration["URLAPI"] + "api/Ads");
                
                var imageContent = new StreamContent(ad.file.OpenReadStream());
                StreamContent streamContent = new StreamContent(ad.file.OpenReadStream());
                var memoryStream = new MemoryStream();
                await streamContent.CopyToAsync(memoryStream);
                var bytes = memoryStream.ToArray();
                string base64 = Convert.ToBase64String(bytes);
                double d = base64.Length;

                AdPost adPost = new AdPost();
                adPost.Id = ad.Id;
                adPost.Titre = ad.Titre;
                adPost.Description = ad.Description;
                adPost.file = base64;
                adPost.fileName = ad.file.FileName;
                adPost.name = ad.file.Name;
                adPost.ContentDisposition = ad.file.ContentDisposition;
                adPost.ContentType = ad.file.ContentType;
                adPost.Link = ad.Link;

                string json = await Task.Run(() => JsonConvert.SerializeObject(adPost));
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                await client.PostAsync(client.BaseAddress, httpContent);
                return RedirectToAction(nameof(Index));
            }

            return View(ad);
        }

        // GET: Ads/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri(Configuration["URLAPI"] + "api/Ads/" + id);

            var ad = await client.GetStringAsync(client.BaseAddress);
            Ad ads = JsonConvert.DeserializeObject<Ad>(ad);
            
            if (ads == null)
            {
                return NotFound();
            }

            AdInput adInput = new AdInput();
            adInput.Id = ads.Id;
            adInput.Titre = ads.Titre;
            adInput.Description = ads.Description;
            adInput.Link = ads.Link;

            return View(adInput);
        }

        // POST: Ads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("Id,Titre,Description,file,Link")] AdInput ad)
        {
            if (id != ad.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string accessToken = await HttpContext.GetTokenAsync("access_token");

                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    client.BaseAddress = new Uri(Configuration["URLAPI"] + "api/Ads/" + ad.Id);

                    var imageContent = new StreamContent(ad.file.OpenReadStream());
                    StreamContent streamContent = new StreamContent(ad.file.OpenReadStream());
                    var memoryStream = new MemoryStream();
                    await streamContent.CopyToAsync(memoryStream);
                    var bytes = memoryStream.ToArray();
                    string base64 = Convert.ToBase64String(bytes);
                    double d = base64.Length;

                    AdPost adPost = new AdPost();
                    adPost.Id = ad.Id;
                    adPost.Titre = ad.Titre;
                    adPost.Description = ad.Description;
                    adPost.file = base64;
                    adPost.fileName = ad.file.FileName;
                    adPost.name = ad.file.Name;
                    adPost.ContentDisposition = ad.file.ContentDisposition;
                    adPost.ContentType = ad.file.ContentType;
                    adPost.Link = ad.Link;

                    string json = await Task.Run(() => JsonConvert.SerializeObject(adPost));
                    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    await client.PutAsync(client.BaseAddress, httpContent);

                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ad);
        }

        // GET: Ads/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri(Configuration["URLAPI"] + "api/Ads/" + id);

            var ad = await client.GetStringAsync(client.BaseAddress);
            Ad ads = JsonConvert.DeserializeObject<Ad>(ad);
            if (ads == null)
            {
                return NotFound();
            }

            return View(ads);
        }

        // POST: Ads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri(Configuration["URLAPI"] + "api/Ads/" + id);
            await client.DeleteAsync(client.BaseAddress);
            return RedirectToAction(nameof(Index));
        }
    }
}
