using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerRequestsController : ControllerBase
    {
        private readonly LoveMirroringContext _context;

        public AnswerRequestsController(LoveMirroringContext context)
        {
            _context = context;
        }

        // GET: api/AnswerRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnswerRequest>>> GetAnswerRequests()
        {
            return await _context.AnswerRequests.ToListAsync();
        }

        // GET: api/AnswerRequests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AnswerRequest>> GetAnswerRequest(short id)
        {
            var answerRequest = await _context.AnswerRequests.FindAsync(id);

            if (answerRequest == null)
            {
                return NotFound();
            }

            return answerRequest;
        }

        // PUT: api/AnswerRequests/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnswerRequest(short id, AnswerRequest answerRequest)
        {
            if (id != answerRequest.AnswerId)
            {
                return BadRequest();
            }

            _context.Entry(answerRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerRequestExists(id))
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

        // POST: api/AnswerRequests
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AnswerRequest>> PostAnswerRequest(AnswerRequest answerRequest)
        {
            _context.AnswerRequests.Add(answerRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnswerRequest", new { id = answerRequest.AnswerId }, answerRequest);
        }

        // DELETE: api/AnswerRequests/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AnswerRequest>> DeleteAnswerRequest(short id)
        {
            var answerRequest = await _context.AnswerRequests.FindAsync(id);
            if (answerRequest == null)
            {
                return NotFound();
            }

            _context.AnswerRequests.Remove(answerRequest);
            await _context.SaveChangesAsync();

            return answerRequest;
        }

        private bool AnswerRequestExists(short id)
        {
            return _context.AnswerRequests.Any(e => e.AnswerId == id);
        }
    }
}
