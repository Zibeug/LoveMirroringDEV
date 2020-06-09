using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using Newtonsoft.Json;
using Unosquare.Swan;

namespace mvc.Controllers
{
    [Authorize(Policy = "Administrateur")]
    public class BotCommandsController : Controller
    {
        private IConfiguration _configuration { get; set; }

        public BotCommandsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: BotCommands
        public async Task<IActionResult> Index()
        {
            try
            {
                // Préparation de l'appel à l'API
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Récurération des données et convertion des données dans le bon type
                string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/BotCommands");
                List<BotCommand> commands = JsonConvert.DeserializeObject<List<BotCommand>>(content);

                return View(commands);
            }
            catch (HttpRequestException)
            {
                return Unauthorized();
            }
        }

        // GET: BotCommands/Details/5
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
                string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/BotCommands/{id}");
                BotCommand botCommand = JsonConvert.DeserializeObject<BotCommand>(content);

                if (botCommand == null)
                {
                    return NotFound();
                }

                return View(botCommand);
            }
            catch (HttpRequestException)
            {
                return Unauthorized();
            }
        }

        // GET: BotCommands/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BotCommands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Slug,Answer")] BotCommand botCommand)
        {
            try
            {
                // Préparation de l'appel à l'API
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                if (ModelState.IsValid)
                {
                    StringContent httpContent = new StringContent(botCommand.ToJson(), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(_configuration["URLAPI"] + "api/BotCommands", httpContent);

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return Unauthorized();
                    }

                    return RedirectToAction(nameof(Index));
                }

                return View(botCommand);
            }
            catch (HttpRequestException)
            {
                return Unauthorized();
            }
        }

        // GET: BotCommands/Edit/5
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
                string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/BotCommands/{id}");

                BotCommand botCommand = JsonConvert.DeserializeObject<BotCommand>(content);

                if (botCommand == null)
                {
                    return NotFound();
                }
                return View(botCommand);
            }
            catch (HttpRequestException)
            {
                return Unauthorized();
            }
        }

        // POST: BotCommands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("Id,Name,Slug,Answer")] BotCommand botCommand)
        {
            try
            {
                if (id != botCommand.Id)
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
                    StringContent httpContent = new StringContent(botCommand.ToJson(), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PutAsync(_configuration["URLAPI"] + $"api/BotCommands/{id}", httpContent);
                    if (response.StatusCode != HttpStatusCode.NoContent)
                    {
                        return BadRequest();
                    }
                    return RedirectToAction(nameof(Index));
                }

                return View(botCommand);
            }
            catch (HttpRequestException)
            {
                return Unauthorized();
            }
        }

        // GET: BotCommands/Delete/5
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
                string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/BotCommands/{id}");
                BotCommand botCommand = JsonConvert.DeserializeObject<BotCommand>(content);

                if (botCommand == null)
                {
                    return NotFound();
                }

                return View(botCommand);
            }
            catch (HttpRequestException)
            {
                return Unauthorized();
            }
        }

        // POST: BotCommands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            try
            {
                // Préparation de l'appel à l'API
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                if (ModelState.IsValid)
                {
                    HttpResponseMessage response = await client.DeleteAsync(_configuration["URLAPI"] + $"api/BotCommands/{id}");

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return BadRequest();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException)
            {
                return Unauthorized();
            }
        }
    }
}
