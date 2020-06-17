/*
 * Auteur : Allemann Tim
 * Date : 16.06.2020
 * Description : Contrôleur pour afficher et traiter les insultes
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Authorize(Policy = "Administrateur")]
    [Route("api/[controller]")]
    [ApiController]
    public class InsultsController : ControllerBase
    {
        private readonly LoveMirroringContext _context;

        public InsultsController(LoveMirroringContext context)
        {
            _context = context;
        }

        // GET: api/Insults
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Insult>>> GetInsult()
        {
            return await _context.Insults.ToListAsync();
        }

        // GET: api/Insults/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Insult>> GetInsult(int id)
        {
            var insult = await _context.Insults.FindAsync(id);

            if (insult == null)
            {
                return NotFound();
            }

            return insult;
        }

        // PUT: api/Insults/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInsult(int id, Insult insult)
        {
            if (id != insult.InsultId)
            {
                return BadRequest();
            }

            _context.Entry(insult).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InsultExists(id))
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

        // POST: api/Insults
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Insult>> PostInsult(Insult insult)
        {
            _context.Insults.Add(insult);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInsult", new { id = insult.InsultId }, insult);
        }

        // DELETE: api/Insults/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Insult>> DeleteInsult(int id)
        {
            var insult = await _context.Insults.FindAsync(id);
            if (insult == null)
            {
                return NotFound();
            }

            _context.Insults.Remove(insult);
            await _context.SaveChangesAsync();

            return insult;
        }

        private bool InsultExists(int id)
        {
            return _context.Insults.Any(e => e.InsultId == id);
        }
    }
}
