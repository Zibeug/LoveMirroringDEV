/*
 * Auteur :Paul Gillet
 * Date : 27.05.2020
 * Description : permet de récupérer la liste des demandes de contact et les afficher dans l'interface administrateur
 */
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Authorize(Policy = "Administrateur")]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactRequestsController : ControllerBase
    {
        private readonly LoveMirroringContext _context;

        public ContactRequestsController(LoveMirroringContext context)
        {
            _context = context;
        }

        // GET: api/ContactRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactRequest>>> GetContactRequests()
        {
            return await _context.ContactRequests
                .Include(x => x.AnswerRequests)
                .ToListAsync();
        }

        // GET: api/ContactRequests/5
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

        // PUT: api/ContactRequests/5
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

        // POST: api/ContactRequests
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ContactRequest>> PostContactRequest(ContactRequest contactRequest)
        {
            _context.ContactRequests.Add(contactRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContactRequest", new { id = contactRequest.RequestId }, contactRequest);
        }

        // DELETE: api/ContactRequests/5
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
