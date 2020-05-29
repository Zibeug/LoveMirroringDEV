using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private IConfiguration Configuration { get; set; }
        private LoveMirroringContext _context;

        public HomeController(IConfiguration configuration, LoveMirroringContext context)
        {
            Configuration = configuration;
            _context = context;
        }

        [Route("GetAds")]
        [HttpGet]
        public async Task<IActionResult> GetAds()
        {
            List<Ad> ads = _context.Ads.ToList();
            return new JsonResult(ads);
        }
    }
}