using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace mvc.Controllers
{
    public class QuizController : Controller
    {
        private HttpClient client = new HttpClient();
        private IConfiguration Configuration { get; }

        public QuizController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [Authorize]
        public async Task<IActionResult> QuizAsync()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string content = await client.GetStringAsync(Configuration["URLAPI"] + "api/Quiz");
            string answerContent = await client.GetStringAsync(Configuration["URLAPI"] + "api/Quiz/answer");
            List<Question> result = JsonConvert.DeserializeObject<List<Question>>(content);
            List<Answer> resultAnswer = JsonConvert.DeserializeObject<List<Answer>>(answerContent);
            //List<Question> questionList = new List<Question>();
            ViewData["questions"] = result;
            ViewData["answer"] = resultAnswer;
            return View();
        }
    }
}