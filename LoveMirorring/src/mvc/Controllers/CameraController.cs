/*
 *      Auteur : Tim Allemann
 *      2020.05.26
 *      Enregistre la photo de l'utilisateur à l'aide de la webcam
 */
 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace mvc.Controllers
{
    public class CameraController : Controller
    {
        [Obsolete]
        private readonly IHostingEnvironment _environment;
        private readonly IConfiguration _configuration;

        [Obsolete]
        public CameraController(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _environment = hostingEnvironment;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Capture()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CaptureAsync(string name)
        {
            // Préparation de l'appel à l'API
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Préparation de la requête update à l'API
            MultipartFormDataContent form = new MultipartFormDataContent();
            HttpContent content = new StringContent("files");
            form.Add(content, "files");

            var files = HttpContext.Request.Form.Files;
            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var stream = file.OpenReadStream();
                        content = new StreamContent(stream);
                        content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "files",
                            FileName = Path.GetExtension(file.FileName)
                        };
                        form.Add(content);

                    }
                }
                HttpResponseMessage response = await client.PostAsync(_configuration["URLAPI"] + "api/Pictures", form);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Json(false);
                }
                return Json(true);
            }
            else
            {
                return Json(false);
            }

        }

        
    }
}