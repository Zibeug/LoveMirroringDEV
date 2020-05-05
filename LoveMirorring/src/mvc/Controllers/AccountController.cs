/*
 *      Auteur : Tim Allemann
 *      2020.04.27
 *      Permet de gérer un compte
 */
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using mvc.Models;
using Newtonsoft.Json;
using Unosquare.Swan;

namespace mvc.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IConfiguration configuration,
                                 ILogger<AccountController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        // Affiche le profil de l'utilisateur
        // GET: Account/Details
        public async Task<IActionResult> Details()
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

            return View(user);
        }

        public async Task<IActionResult> GetUserInfoInJson()
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Récurération des données et convertion des données dans le bon type
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Account/getUserInfo");
            AspNetUser user = JsonConvert.DeserializeObject<AspNetUser>(content);

            Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalData.json");
            return new FileContentResult(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(user)), "text/json");
        }

        // Met à jour le profil de l'utilisateur
        // GET: Account/Edit/5
        public async Task<IActionResult> Edit(string id)
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
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Account/getUserInfo");
            AspNetUser aspNetUser = JsonConvert.DeserializeObject<AspNetUser>(content);

            if (aspNetUser == null)
            {
                return NotFound();
            }

            // Récurération des données et convertion des données dans le bon type, idem que précédemment
            content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Data/corpulences");
            List<Corpulence> corpulences = JsonConvert.DeserializeObject<List<Corpulence>>(content);
            ViewData["CorpulenceId"] = new SelectList(corpulences, "CorpulenceId", "CorpulenceName", aspNetUser.CorpulenceId);

            content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Data/hairColor");
            List<HairColor> hairColors = JsonConvert.DeserializeObject<List<HairColor>>(content);
            ViewData["HairColorId"] = new SelectList(hairColors, "HairColorId", "HairColorName", aspNetUser.HairColorId);

            content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Data/hairSize");
            List<HairSize> hairSizes = JsonConvert.DeserializeObject<List<HairSize>>(content);
            ViewData["HairSizeId"] = new SelectList(hairSizes, "HairSizeId", "HairSizeName", aspNetUser.HairSizeId);

            content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Data/sex");
            List<Sex> sexs = JsonConvert.DeserializeObject<List<Sex>>(content);
            ViewData["SexeId"] = new SelectList(sexs, "SexeId", "SexeName", aspNetUser.SexeId);

            content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Data/sexuality");
            List<Sexuality> sexualitiess = JsonConvert.DeserializeObject<List<Sexuality>>(content);
            ViewData["SexualityId"] = new SelectList(sexualitiess, "SexualityId", "SexualityName", aspNetUser.SexualityId);

            content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Data/subscription");
            List<Subscription> subscriptions = JsonConvert.DeserializeObject<List<Subscription>>(content);
            ViewData["SubscriptionId"] = new SelectList(subscriptions, "SubscriptionId", "SubscriptionName", aspNetUser.SubscriptionId);

            return View(aspNetUser);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,HairColorId,CorpulenceId,SexeId,HairSizeId,SubscriptionId,SexualityId,AccessFailedCount,ConcurrencyStamp,Email,EmailConfirmed,LockoutEnabled,LockoutEnd,NormalizedEmail,NormalizedUserName,PasswordHash,PhoneNumber,PhoneNumberConfirmed,SecurityStamp,TwoFactorEnabled,UserName,Birthday,Firstname,LastName,QuizCompleted")] AspNetUser aspNetUser)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Récurération des données et convertion des données dans le bon type
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Account/getUserInfo");
            AspNetUser aspNetUserFromClaim = JsonConvert.DeserializeObject<AspNetUser>(content);

            if (id != aspNetUser.Id || id != aspNetUserFromClaim.Id)
            {
                return NotFound();
            }

            // Changement des données par rapport à l'utilisateur selon le claim, pour éviter qu'un pirate ne change un autre user
            aspNetUserFromClaim.Email = aspNetUser.Email;
            aspNetUserFromClaim.PhoneNumber = aspNetUser.PhoneNumber;
            aspNetUserFromClaim.Firstname = aspNetUser.Firstname;
            aspNetUserFromClaim.LastName = aspNetUser.LastName;
            aspNetUserFromClaim.CorpulenceId = aspNetUser.CorpulenceId;
            aspNetUserFromClaim.HairColorId = aspNetUser.HairColorId;
            aspNetUserFromClaim.HairSizeId = aspNetUser.HairSizeId;
            aspNetUserFromClaim.SexeId = aspNetUser.SexeId;
            aspNetUserFromClaim.SexualityId = aspNetUser.SexualityId;

            if (ModelState.IsValid)
            {
                // Préparation de la requête update à l'API
                StringContent httpContent = new StringContent(aspNetUserFromClaim.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(_configuration["URLAPI"] + "api/Account/PutUser", httpContent);
                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    return BadRequest();
                }

                return RedirectToAction(nameof(Details));
            }

            // En cas d'erreur de modèle, il faut refournir à la vue les données...
            content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Data/corpulences");
            List<Corpulence> corpulences = JsonConvert.DeserializeObject<List<Corpulence>>(content);
            ViewData["CorpulenceId"] = new SelectList(corpulences, "CorpulenceId", "CorpulenceName", aspNetUser.CorpulenceId);

            content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Data/hairColor");
            List<HairColor> hairColors = JsonConvert.DeserializeObject<List<HairColor>>(content);
            ViewData["HairColorId"] = new SelectList(hairColors, "HairColorId", "HairColorName", aspNetUser.HairColorId);

            content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Data/hairSize");
            List<HairSize> hairSizes = JsonConvert.DeserializeObject<List<HairSize>>(content);
            ViewData["HairSizeId"] = new SelectList(hairSizes, "HairSizeId", "HairSizeName", aspNetUser.HairSizeId);

            content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Data/sex");
            List<Sex> sexs = JsonConvert.DeserializeObject<List<Sex>>(content);
            ViewData["SexeId"] = new SelectList(sexs, "SexeId", "SexeName", aspNetUser.SexeId);

            content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Data/sexuality");
            List<Sexuality> sexualitiess = JsonConvert.DeserializeObject<List<Sexuality>>(content);
            ViewData["SexualityId"] = new SelectList(sexualitiess, "SexualityId", "SexualityName", aspNetUser.SexualityId);

            content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Data/subscription");
            List<Subscription> subscriptions = JsonConvert.DeserializeObject<List<Subscription>>(content);
            ViewData["SubscriptionId"] = new SelectList(subscriptions, "SubscriptionId", "SubscriptionName", aspNetUser.SubscriptionId);

            return View(aspNetUser);
        }

        // GET: Account/Delete/5
        public async Task<IActionResult> Delete()
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Récurération des données et convertion des données dans le bon type
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
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Envoie de la demande du suppression du compte
            HttpResponseMessage content = await client.DeleteAsync(_configuration["URLAPI"] + "api/Account/");

            if (content.StatusCode == HttpStatusCode.OK)
            {
                return SignOut("Cookies", "oidc");
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
