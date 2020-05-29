﻿/*
 * Auteur : Gillet Paul
 * Date : 26.05.2020
 * Description : Contrôleur pour afficher et traiter les profils
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
    public class ProfilsController : Controller
    {
        private readonly IConfiguration _configuration;

        public ProfilsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: Profils
        public async Task<IActionResult> Index()
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Récurération des données et convertion des données dans le bon type
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Profils");
            List<Profil> profils = JsonConvert.DeserializeObject<List<Profil>>(content);

            return View(profils);
        }

        // GET: Profils/Details/5
        public async Task<IActionResult> Details(short? id)
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
            string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Profils/{id}");
            Profil profil = JsonConvert.DeserializeObject<Profil>(content);

            if (profil == null)
            {
                return NotFound();
            }

            return View(profil);
        }

        // GET: Profils/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Profils/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProfilId,ProfilName,ProfilDescription")] Profil profil)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (ModelState.IsValid)
            {
                StringContent httpContent = new StringContent(profil.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(_configuration["URLAPI"] + "api/Profils", httpContent);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(profil);
        }

        // GET: Profils/Edit/5
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

            // Récurération des données et convertion des données dans le bon type
            string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Profils/{id}");

            Profil profil = JsonConvert.DeserializeObject<Profil>(content); 

            if (profil == null)
            {
                return NotFound();
            }
            return View(profil);
        }

        // POST: Profils/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("ProfilId,ProfilName,ProfilDescription")] Profil profil)
        {
            if (id != profil.ProfilId)
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
                StringContent httpContent = new StringContent(profil.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(_configuration["URLAPI"] + $"api/Profils/{id}", httpContent);
                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    return BadRequest();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(profil);
        }

        // GET: Profils/Delete/5
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
            string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Profils/{id}");
            Profil profil = JsonConvert.DeserializeObject<Profil>(content);

            if (profil == null)
            {
                return NotFound();
            }

            return View(profil);
        }

        // POST: Profils/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await client.DeleteAsync(_configuration["URLAPI"] + $"api/Profils/{id}");

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return BadRequest();
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
