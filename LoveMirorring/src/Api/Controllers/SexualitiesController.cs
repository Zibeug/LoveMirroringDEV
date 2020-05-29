/*
 * Auteur : Gillet Paul
 * Date : 26.05.2020
 * Description : Contrôleur pour afficher et traiter les sexualités
 */

using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SexualitiesController : ControllerBase
    {
        private readonly LoveMirroringContext _context;

        public SexualitiesController(LoveMirroringContext context)
        {
            _context = context;
        }

        // GET: api/Sexualities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sexuality>>> GetSexualities()
        {
            return await _context.Sexualities.ToListAsync();
        }

        // GET: api/Sexualities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sexuality>> GetSexuality(short id)
        {
            var sexuality = await _context.Sexualities.FindAsync(id);

            if (sexuality == null)
            {
                return NotFound();
            }

            return sexuality;
        }

        // PUT: api/Sexualities/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSexuality(short id, Sexuality sexuality)
        {
            if (id != sexuality.SexualityId)
            {
                return BadRequest();
            }

            _context.Entry(sexuality).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SexualityExists(id))
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

        // POST: api/Sexualities
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Sexuality>> PostSexuality(Sexuality sexuality)
        {
            _context.Sexualities.Add(sexuality);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSexuality", new { id = sexuality.SexualityId }, sexuality);
        }

        // DELETE: api/Sexualities/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Sexuality>> DeleteSexuality(short id)
        {
            var sexuality = await _context.Sexualities.FindAsync(id);
            if (sexuality == null)
            {
                return NotFound();
            }

            _context.Sexualities.Remove(sexuality);
            await _context.SaveChangesAsync();

            return sexuality;
        }

        private bool SexualityExists(short id)
        {
            return _context.Sexualities.Any(e => e.SexualityId == id);
        }
    }
}