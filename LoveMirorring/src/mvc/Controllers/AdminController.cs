using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using mvc.ViewModels.Admin;
using Newtonsoft.Json;

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

            return View();
        }

        public async Task<IActionResult> Search()
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

            return View();
        }
    }
}