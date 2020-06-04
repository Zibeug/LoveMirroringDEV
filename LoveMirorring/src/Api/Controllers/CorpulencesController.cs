/*
 * Auteur : Gillet Paul
 * Date : 26.05.2020
 * Description : Contrôleur pour afficher et traiter les corpulences
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
    public class CorpulencesController : ControllerBase
    {
        private readonly LoveMirroringContext _context;

        public CorpulencesController(LoveMirroringContext context)
        {
            _context = context;
        }

        // GET: api/Corpulences
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Corpulence>>> GetCorpulences()
        {
            return await _context.Corpulences.ToListAsync();
        }

        // GET: api/Corpulences/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Corpulence>> GetCorpulence(short id)
        {
            var corpulence = await _context.Corpulences.FindAsync(id);

            if (corpulence == null)
            {
                return NotFound();
            }

            return corpulence;
        }

        // PUT: api/Corpulences/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCorpulence(short id, Corpulence corpulence)
        {
            if (id != corpulence.CorpulenceId)
            {
                return BadRequest();
            }

            _context.Entry(corpulence).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CorpulenceExists(id))
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

        // POST: api/Corpulences
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Corpulence>> PostCorpulence(Corpulence corpulence)
        {
            _context.Corpulences.Add(corpulence);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCorpulence", new { id = corpulence.CorpulenceId }, corpulence);
        }

        // DELETE: api/Corpulences/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Corpulence>> DeleteCorpulence(short id)
        {
            var corpulence = await _context.Corpulences.FindAsync(id);
            if (corpulence == null)
            {
                return NotFound();
            }

            _context.Corpulences.Remove(corpulence);
            await _context.SaveChangesAsync();

            return corpulence;
        }

        private bool CorpulenceExists(short id)
        {
            return _context.Corpulences.Any(e => e.CorpulenceId == id);
        }
    }
}
