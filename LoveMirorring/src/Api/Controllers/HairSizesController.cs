/*
 * Auteur : Gillet Paul
 * Date : 26.05.2020
 * Description : Contrôleur pour afficher et traiter les tailles de cheveux
 */

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
    public class HairSizesController : ControllerBase
    {
        private readonly LoveMirroringContext _context;

        public HairSizesController(LoveMirroringContext context)
        {
            _context = context;
        }

        // GET: api/HairSizes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HairSize>>> GetHairSizes()
        {
            return await _context.HairSizes.ToListAsync();
        }

        // GET: api/HairSizes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HairSize>> GetHairSize(short id)
        {
            var hairSize = await _context.HairSizes.FindAsync(id);

            if (hairSize == null)
            {
                return NotFound();
            }

            return hairSize;
        }

        // PUT: api/HairSizes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHairSize(short id, HairSize hairSize)
        {
            if (id != hairSize.HairSizeId)
            {
                return BadRequest();
            }

            _context.Entry(hairSize).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HairSizeExists(id))
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

        // POST: api/HairSizes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<HairSize>> PostHairSize(HairSize hairSize)
        {
            _context.HairSizes.Add(hairSize);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHairSize", new { id = hairSize.HairSizeId }, hairSize);
        }

        // DELETE: api/HairSizes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<HairSize>> DeleteHairSize(short id)
        {
            var hairSize = await _context.HairSizes.FindAsync(id);
            if (hairSize == null)
            {
                return NotFound();
            }

            _context.HairSizes.Remove(hairSize);
            await _context.SaveChangesAsync();

            return hairSize;
        }

        private bool HairSizeExists(short id)
        {
            return _context.HairSizes.Any(e => e.HairSizeId == id);
        }
    }
}
