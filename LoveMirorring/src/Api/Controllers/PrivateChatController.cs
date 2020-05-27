using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Api.Models;
using Api.ViewModels.PrivateChat;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PrivateChatController : ControllerBase
    {
        private readonly LoveMirroringContext _context;
        private IConfiguration Configuration { get; set; }

        public PrivateChatController(LoveMirroringContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }


        [Route("GetMatch")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetLike()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            AspNetUser currentuser = null;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string userString = await client.GetStringAsync(Configuration["URLAPI"] + "api/Account/getUserInfo");
            currentuser = JsonConvert.DeserializeObject<AspNetUser>(userString);

            List<AspNetUser> othersuser = await _context.AspNetUsers.Where(u => u.Id != currentuser.Id).ToListAsync();
            List<AspNetUser> usermatch = new List<AspNetUser>();

            foreach (AspNetUser user in othersuser)
            {
                UserLike likeCurrentUser = _context.UserLikes.Where(d => d.Id == currentuser.Id && d.Id1 == user.Id).SingleOrDefault();
                UserLike likeUser = _context.UserLikes.Where(d => d.Id == user.Id && d.Id1 == currentuser.Id).SingleOrDefault();

                if (likeCurrentUser != null && likeUser != null)
                {
                    usermatch.Add(user);
                }
            }

            return new JsonResult(usermatch);
        }



        [Route("CreateMessage")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateMessage(Message message)
        {
            if(message == null)
            {
                return BadRequest();
            }
            else
            {
                if (message.MessageText != "")
                {
                    message.MessageDate = DateTime.Now;
                    _context.Messages.Add(message);
                    await _context.SaveChangesAsync();
                }

                return Ok();
            }
        }


        [Route("GetTalk/{id}")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetTalk(string id)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            AspNetUser user = null;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string userString = await client.GetStringAsync(Configuration["URLAPI"] + "api/Account/getUserInfo");
            user = JsonConvert.DeserializeObject<AspNetUser>(userString);
            
            Talk talk = null;
            talk = await _context.Talks.Where(t => t.Id == user.Id && t.IdUser2Talk == id).FirstOrDefaultAsync();
            if(talk == null)
            {
                talk = await _context.Talks.Where(t => t.Id == id && t.IdUser2Talk == user.Id).FirstOrDefaultAsync();
            }
            return new JsonResult(talk);
        }

        [Route("GetMessages/{id}")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMessages(int id)
        {
            Talk talk = await _context.Talks.Where(t => t.TalkId == id).SingleAsync();
            IEnumerable<GetMessagesViewModel> messages = from m in await _context.Messages.ToListAsync()
                                                  join t in await _context.Talks.ToListAsync() on m.TalkId equals t.TalkId
                                                         where t.TalkId == id
                                                  join u in await _context.AspNetUsers.ToListAsync() on m.Id equals u.Id
                                                  select new GetMessagesViewModel {Username = u.UserName, Message = m.MessageText, Date = m.MessageDate};
            return new JsonResult(messages);
        }
    }
}