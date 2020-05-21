/*
 *      Auteur : Hans Morsch
 *      11.05.2020
 *      Contrôleur Api pour l'admin
 *      Permet de gérer les utilisateurs et rôles
 */

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using mvc.ViewModels;
using mvc.ViewModels.Admin;
using Newtonsoft.Json;
using Unosquare.Swan;


namespace mvc.Controllers
{
    [Authorize(Policy = "Administrateur")]
    public class AdminController : Controller
    {
        private IConfiguration _configuration { get; }

        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Préparation de l'appel à l'API
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                if(client.DefaultRequestHeaders.Authorization == null)
                {
                    throw new AccessViolationException();
                }

                string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Admin/Welcome");

                if (content == null)
                {
                    throw new Exception("Api access error");
                
                }

                IndexModel overView = JsonConvert.DeserializeObject<IndexModel>(content);

                if (overView == null)
                {
                    throw new Exception("Internal Server Error");
                }

                return View(overView);
            }
            catch(AccessViolationException)
            {
                return View("~/Home/Error"); 
            }
        
        }

        public async Task<IActionResult> Search(string username)
        {
            try
            {
                string accessToken = await HttpContext.GetTokenAsync("access_token");

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                string content = "";
                string id = "";

                if (!String.IsNullOrEmpty(username))
                {
                    content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Admin/Search/{username}");
                    id = JsonConvert.DeserializeObject<string>(content);

                    if (id == null)
                    {
                        ModelState.AddModelError(string.Empty, $"{username} does not exist.");
                        return View();
                    }
                    content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Admin/GetUser/{id}");

                    SearchModel search = JsonConvert.DeserializeObject<SearchModel>(content);

                    if (search == null)
                    {
                        return View();
                    }

                    return View(search);
                }

                return View();
            }
            catch (Exception)
            {
                return View("Home");

            }
        }

        /*
         * Auteur : Sébastien Berger 
         * Date : 18.05.2020
         * Description : récupérer les utilisateurs depuis l'API et les afficher dans la vue.
         */
        public async Task<IActionResult> GetAllUsers()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Admin/GetAllUsers");
            List<AspNetUser> users = JsonConvert.DeserializeObject<List<AspNetUser>>(content);

            ViewData["users"] = users;
            return View("Users");
        }

        /*
         * Auteur : Sébastien Berger
         * Date : 18.05.2020
         * Description : récupérer les utilisateur qui ont été banni depuis l'API et les afficher dans la vue.
         */
        public async Task<IActionResult> GetAllBan()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Admin/GetAllBan");
            List<AspNetUser> users = JsonConvert.DeserializeObject<List<AspNetUser>>(content);

            ViewData["users"] = users;
            return View("BannedUsers");
        }

        /*
         * Auteur : Sébastien Berger
         * Date: 18.05.2020
         * Description : Enlever le ban d'un utilisateur directement depuis l'interface administrateur
         */
        public async Task<IActionResult> UnBan(string id)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            StringContent httpContent = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(_configuration["URLAPI"] + "api/Admin/UnBan", httpContent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                await Index();
                return View("Index");
            }
            else
            {
                return BadRequest();
            }
        }

        /*
         * Auteur : Sébastien Berger
         * Date : 18.05.2020
         * Description : permet de récuperer les utilisateurs qui ont répondu au Quiz pour pouvoir le reset
         */
        public async Task<IActionResult> ViewQuiz()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Admin/GetAllQuiz");
            List<AspNetUser> users = JsonConvert.DeserializeObject<List<AspNetUser>>(content);

            ViewData["users"] = users;
            return View("Quiz");
        }

        /*
         * Auteur : Sébastien Berger
         * Date : 18.05.2020
         * Description : permet de Reset le Quiz d'un utilisateur en cas de problème ou si l'utilisateur souhaite changer son profil.
         */
        public async Task<IActionResult> ResetQuiz(string id)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            HttpResponseMessage response = await client.DeleteAsync(_configuration["URLAPI"] + "api/Admin/ResetQuiz/" + id);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                await Index();
                return View("Index");
            }
            else
            {
                return BadRequest();
            }
        }

        /*
         * Auteur : Sébastien Berger 
         * Date : 18.05.2020
         * Description : permet d'afficher les comptes qui n'ont pas été encore complétement validé
         */
        public async Task<IActionResult> Validation()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Admin/UserToValidate");
            List<AspNetUser> users = JsonConvert.DeserializeObject<List<AspNetUser>>(content);

            ViewData["users"] = users;
            return View("Validation");
        }

        public async Task<IActionResult> ValidationAccount(string id)
        {
            return null;
        }

        public async Task<IActionResult> Details(string id)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (!String.IsNullOrEmpty(id))
            {
                string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Admin/Details/{id}");

                AspNetUser user = JsonConvert.DeserializeObject<AspNetUser>(content);

                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }

            return View("Search");
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Admin/Details/{id}");
            AspNetUser aspNetUser = JsonConvert.DeserializeObject<AspNetUser>(content);

            if (aspNetUser == null)
            {
                return NotFound();
            }

            return View(aspNetUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Email,EmailConfirmed,NormalizedEmail,AccessFailedCount,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,QuizCompleted,LockoutEnabled,LockoutEnd,UserName,NormalizedUserName,SecurityStamp,Birthday")] AspNetUser aspNetUser)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (!String.IsNullOrEmpty(id))
            {
                string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Admin/Details/{id}");

                AspNetUser user = JsonConvert.DeserializeObject<AspNetUser>(content);

                if (user == null || !user.Id.Equals(id) || !aspNetUser.Id.Equals(id))
                {
                    return NotFound();
                }

                user.Email = aspNetUser.Email;
                user.EmailConfirmed = aspNetUser.EmailConfirmed;
                user.NormalizedEmail = aspNetUser.NormalizedEmail;
                user.AccessFailedCount = aspNetUser.AccessFailedCount;
                user.ConcurrencyStamp = aspNetUser.ConcurrencyStamp;
                user.PhoneNumber = aspNetUser.PhoneNumber;
                user.PhoneNumberConfirmed = aspNetUser.PhoneNumberConfirmed;
                user.QuizCompleted = aspNetUser.QuizCompleted;
                user.LockoutEnabled = aspNetUser.LockoutEnabled;
                user.LockoutEnd = aspNetUser.LockoutEnd;
                user.UserName = aspNetUser.UserName;
                user.NormalizedUserName = aspNetUser.NormalizedUserName;
                user.SecurityStamp = aspNetUser.SecurityStamp;
                user.Birthday = aspNetUser.Birthday;

                if (ModelState.IsValid)
                {
                    StringContent httpContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PutAsync(_configuration["URLAPI"] + "api/Admin/Edit", httpContent);
                    if (response.StatusCode != HttpStatusCode.NoContent)
                    {
                        return BadRequest();
                    }

                    return RedirectToAction(nameof(Details));
                }
                
                return View(aspNetUser);
            }

            return View("Search");
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Admin/Details/{id}");
            AspNetUser aspNetUser = JsonConvert.DeserializeObject<AspNetUser>(content);

            if (aspNetUser == null)
            {
                return NotFound();
            }

            return View(aspNetUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (!String.IsNullOrEmpty(id))
            {
                HttpResponseMessage content = await client.DeleteAsync(_configuration["URLAPI"] + $"api/Admin/Delete/{id}");

                if (content.StatusCode == HttpStatusCode.OK)
                {
                    return View("Search");
                }
                else
                {
                    return BadRequest();
                }
            }

            return View("Search");
        }


        public async Task<IActionResult> Roles()
        {

            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Admin/Roles");
            RolesModel roles = JsonConvert.DeserializeObject<RolesModel>(content);

            if (roles == null)
            {
                return NotFound();
            }

            return View(roles);
        }

        public async Task<IActionResult> CreateRole(AspNetRole role)
        {

            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (ModelState.IsValid)
            {
                StringContent httpContent = new StringContent(role.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(_configuration["URLAPI"] + $"api/Admin/CreateRole", httpContent);
                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    return BadRequest();
                }

                return RedirectToAction(nameof(Roles));
            }

            return View();
        }

        public async Task<IActionResult> AddUserToRole(UpdateUserRoleModel model)
        {

            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (ModelState.IsValid)
            {
                StringContent httpContent = new StringContent(model.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(_configuration["URLAPI"] + $"api/Admin/UpdateUserRole", httpContent);
                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    return BadRequest();
                }

                return RedirectToAction(nameof(Roles));
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GiveNewPassword(AspNetUser user)
        {

            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            StringContent httpContent = new StringContent(user.ToJson(), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(_configuration["URLAPI"] + $"api/Admin/GiveNewPassword/{user.Id}", httpContent);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return BadRequest();
            }

            return RedirectToAction("Search");

        }

        public async Task<IActionResult> GiveNewPassword(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Admin/Details/{id}");
            AspNetUser aspNetUser = JsonConvert.DeserializeObject<AspNetUser>(content);

            if (aspNetUser == null)
            {
                return NotFound();
            }

            return View(aspNetUser);
        }
    }
}