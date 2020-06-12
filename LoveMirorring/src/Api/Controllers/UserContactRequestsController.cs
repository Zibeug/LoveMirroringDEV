using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;
using SpotifyAPI.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserContactRequestsController : ControllerBase
    {
        private readonly LoveMirroringContext _context;

        public UserContactRequestsController(LoveMirroringContext context)
        {
            _context = context;
        }  

        // GET: api/UserContactRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactRequest>>> GetContactRequests()
        {
            string id = "";
            try
            {
                // Il faut utiliser le Claim pour retrouver l'identifiant de l'utilisateur
                id = User.Claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").SingleOrDefault().Value;
            }
            catch (Exception)
            {
                return BadRequest();
            }

            if (id == null)
            {
                return BadRequest();
            }

            return await _context.ContactRequests
                .Include(x=> x.AnswerRequests)
                .Where(x => x.Id == id)
                .ToListAsync();
        }

        // GET: api/UserContactRequests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactRequest>> GetContactRequest(short id)
        {
            var contactRequest = await _context.ContactRequests.FindAsync(id);

            if (contactRequest == null)
            {
                return NotFound();
            }

            return contactRequest;
        }

        // PUT: api/UserContactRequests/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContactRequest(short id, ContactRequest contactRequest)
        {
            if (id != contactRequest.RequestId)
            {
                return BadRequest();
            }

            _context.Entry(contactRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactRequestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserContactRequests
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ContactRequest>> PostContactRequest(ContactRequest contactRequest)
        {
            _context.ContactRequests.Add(contactRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContactRequest", new { id = contactRequest.RequestId }, contactRequest);
        }

        // DELETE: api/UserContactRequests/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ContactRequest>> DeleteContactRequest(short id)
        {
            var contactRequest = await _context.ContactRequests.FindAsync(id);
            if (contactRequest == null)
            {
                return NotFound();
            }

            _context.ContactRequests.Remove(contactRequest);
            await _context.SaveChangesAsync();

            return contactRequest;
        }

        private bool ContactRequestExists(short id)
        {
            return _context.ContactRequests.Any(e => e.RequestId == id);
        }
    }
}
