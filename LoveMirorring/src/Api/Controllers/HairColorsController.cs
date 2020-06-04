/*
 * Auteur : Gillet Paul
 * Date : 18.05.2020
 * Description : Contrôleur pour afficher et traiter les couleurs de cheveux
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
    public class HairColorsController : ControllerBase
    {
        private readonly LoveMirroringContext _context;

        public HairColorsController(LoveMirroringContext context)
        {
            _context = context;
        }

        // GET: api/HairColors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HairColor>>> GetHairColors()
        {
            return await _context.HairColors.ToListAsync();
        }

        // GET: api/HairColors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HairColor>> GetHairColor(short id)
        {
            var hairColor = await _context.HairColors.FindAsync(id);

            if (hairColor == null)
            {
                return NotFound();
            }

            return hairColor;
        }

        // PUT: api/HairColors/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHairColor(short id, HairColor hairColor)
        {
            if (id != hairColor.HairColorId)
            {
                return BadRequest();
            }

            _context.Entry(hairColor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HairColorExists(id))
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

        // POST: api/HairColors
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<HairColor>> PostHairColor(HairColor hairColor)
        {
            _context.HairColors.Add(hairColor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHairColor", new { id = hairColor.HairColorId }, hairColor);
        }

        // DELETE: api/HairColors/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<HairColor>> DeleteHairColor(short id)
        {
            var hairColor = await _context.HairColors.FindAsync(id);
            if (hairColor == null)
            {
                return NotFound();
            }

            _context.HairColors.Remove(hairColor);
            await _context.SaveChangesAsync();

            return hairColor;
        }

        private bool HairColorExists(short id)
        {
            return _context.HairColors.Any(e => e.HairColorId == id);
        }
    }
}
