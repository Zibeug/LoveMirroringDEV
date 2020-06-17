using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BotActionsController : ControllerBase
    {
        private readonly LoveMirroringContext _context;
        private readonly IEmailSender _emailSender;
        private IConfiguration Configuration { get; set; }

        public BotActionsController(LoveMirroringContext context, IEmailSender emailSender, IConfiguration configuration)
        {
            _context = context;
            _emailSender = emailSender;
            Configuration = configuration;
        }

        //Put : api/BotActions/BanUser/seb
        // Permet de bannir un utilisateur depuis le bot
        [Route("BanUser/{id}")]
        [HttpPut]
        public async Task<IActionResult> BanUser(string id)
        {
            if (id != null)
            {
                AspNetUser user = await _context.AspNetUsers.Where(u => u.UserName == id).FirstOrDefaultAsync();
                user.LockoutEnd = new DateTimeOffset(new DateTime(2400, 05, 03));
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return StatusCode(200);
            }
            else
            {
                return StatusCode(404);
            }
        }
    }
}