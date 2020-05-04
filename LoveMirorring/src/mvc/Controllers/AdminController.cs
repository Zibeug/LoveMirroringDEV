using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using mvc.ViewModels;
using mvc.ViewModels.Admin;
using Newtonsoft.Json;
using Unosquare.Swan;

namespace mvc.Controllers
{
    public class AdminController : Controller
    {
        private IConfiguration _configuration { get; }

        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string content = await client.GetStringAsync(_configuration["URLAPI"] + "api/Admin/welcom");

            IndexModel overView = JsonConvert.DeserializeObject<IndexModel>(content);

            if (overView == null) 
            {
                return NotFound();
            }

            return View(overView);
        }

        public async Task<IActionResult> Search(string username)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string content = "";
            string id = "";

            if (!String.IsNullOrEmpty(username)) 
            {
                content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Admin/search/{username}");
                id = JsonConvert.DeserializeObject<string>(content);

                if (id == null)
                {
                    return NotFound();
                }
                content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Admin/user/{id}");

                SearchModel search = JsonConvert.DeserializeObject<SearchModel>(content);

                if (search == null)
                {
                    return View();
                }

                return View(search);
            }

            return View();
        }

        public async Task<IActionResult> Details(string id)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (!String.IsNullOrEmpty(id))
            {
                string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Admin/details/{id}");

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

            string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Admin/details/{id}");
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
                string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Admin/details/{id}");

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
                    StringContent httpContent = new StringContent(user.ToJson(), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PutAsync(_configuration["URLAPI"] + $"api/Admin/edit", httpContent);
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

            string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/Admin/details/{id}");
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
                HttpResponseMessage content = await client.DeleteAsync(_configuration["URLAPI"] + $"api/Admin/delete/{id}");

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
    }
}