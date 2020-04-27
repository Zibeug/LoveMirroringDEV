/*
 *      Auteur : Tim Allemann
 *      2020.04.27
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using Newtonsoft.Json;

namespace mvc.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly LoveMirroringContext _context;
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: Account/Details/5
        public async Task<IActionResult> Details()
        {

            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Account/getUserInfo");

            AspNetUser user = JsonConvert.DeserializeObject<AspNetUser>(content);         

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Account/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUser = await _context.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return NotFound();
            }
            ViewData["CorpulenceId"] = new SelectList(_context.Corpulences, "CorpulenceId", "CorpulenceName", aspNetUser.CorpulenceId);
            ViewData["HairColorId"] = new SelectList(_context.HairColors, "HairColorId", "HairColorName", aspNetUser.HairColorId);
            ViewData["HairSizeId"] = new SelectList(_context.HairSizes, "HairSizeId", "HairSizeName", aspNetUser.HairSizeId);
            ViewData["SexeId"] = new SelectList(_context.Sexes, "SexeId", "SexeName", aspNetUser.SexeId);
            ViewData["SexualityId"] = new SelectList(_context.Sexualities, "SexualityId", "SexualityName", aspNetUser.SexualityId);
            ViewData["SubscriptionId"] = new SelectList(_context.Subscriptions, "SubscriptionId", "SubscriptionName", aspNetUser.SubscriptionId);
            return View(aspNetUser);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,HairColorId,CorpulenceId,SexeId,HairSizeId,SubscriptionId,SexualityId,AccessFailedCount,ConcurrencyStamp,Email,EmailConfirmed,LockoutEnabled,LockoutEnd,NormalizedEmail,NormalizedUserName,PasswordHash,PhoneNumber,PhoneNumberConfirmed,SecurityStamp,TwoFactorEnabled,UserName,Birthday,Firstname,LastName,QuizCompleted")] AspNetUser aspNetUser)
        {
            if (id != aspNetUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aspNetUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AspNetUserExists(aspNetUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CorpulenceId"] = new SelectList(_context.Corpulences, "CorpulenceId", "CorpulenceName", aspNetUser.CorpulenceId);
            ViewData["HairColorId"] = new SelectList(_context.HairColors, "HairColorId", "HairColorName", aspNetUser.HairColorId);
            ViewData["HairSizeId"] = new SelectList(_context.HairSizes, "HairSizeId", "HairSizeName", aspNetUser.HairSizeId);
            ViewData["SexeId"] = new SelectList(_context.Sexes, "SexeId", "SexeName", aspNetUser.SexeId);
            ViewData["SexualityId"] = new SelectList(_context.Sexualities, "SexualityId", "SexualityName", aspNetUser.SexualityId);
            ViewData["SubscriptionId"] = new SelectList(_context.Subscriptions, "SubscriptionId", "SubscriptionName", aspNetUser.SubscriptionId);
            return View(aspNetUser);
        }

        // GET: Account/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Account/getUserInfo");

            AspNetUser user = JsonConvert.DeserializeObject<AspNetUser>(content);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            HttpResponseMessage content = await client.DeleteAsync(_configuration["URLAPI"] + "api/Account/" + id);

            if (content.StatusCode == HttpStatusCode.OK)
            {
                return SignOut("Cookies", "oidc");
            }
            else
            {
                return BadRequest();
            }        
        }

        private bool AspNetUserExists(string id)
        {
            return _context.AspNetUsers.Any(e => e.Id == id);
        }
    }
}
