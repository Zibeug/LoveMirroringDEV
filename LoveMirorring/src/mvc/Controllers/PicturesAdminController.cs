/*
 *      Auteur : Tim Allemann
 *      15.05.2020
 *      Contrôleur Api pour la validation des images utilisateurs
 *      Permet de gérer les photos des utilisateurs (Admin seulement)
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using Newtonsoft.Json;
using Unosquare.Swan;

namespace mvc.Controllers
{
    public class PicturesAdminController : Controller
    {
        private readonly IConfiguration _configuration;

        public PicturesAdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: PicturesAdmin
        public async Task<IActionResult> Index()
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/PicturesAdmin");
            List<Picture> pictures = JsonConvert.DeserializeObject<List<Picture>>(content);

            ViewData["URLAPI"] = _configuration["URLAPI"];

            return View(pictures);
        }

        // GET: PicturesAdmin/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/PicturesAdmin/" + id);
            Picture picture = JsonConvert.DeserializeObject<Picture>(content);

            if (picture == null)
            {
                return NotFound();
            }

            ViewData["URLAPI"] = _configuration["URLAPI"];

            return View(picture);
        }

        // POST: PicturesAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short pictureid, [Bind("PictureId,Id,PictureView,PictureConfirmed")] Picture picture)
        {
            if (pictureid != picture.PictureId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Préparation de l'appel à l'API
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Préparation de la requête update à l'API
                StringContent httpContent = new StringContent(picture.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(_configuration["URLAPI"] + "api/PicturesAdmin/" + pictureid, httpContent);
                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    return BadRequest();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(picture);
        }

        // GET: PicturesAdmin/Delete/5
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

            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/PicturesAdmin/" + id);
            Picture picture = JsonConvert.DeserializeObject<Picture>(content);

            if (picture == null)
            {
                return NotFound();
            }

            return View(picture);
        }

        // POST: PicturesAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Envoie de la demande du suppression du compte
            HttpResponseMessage content = await client.DeleteAsync(_configuration["URLAPI"] + "api/PicturesAdmin/" + id);

            if (content.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest();
            }

        }

    }
}
