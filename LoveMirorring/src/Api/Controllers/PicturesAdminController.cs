/*
 *      Auteur : Tim Allemann
 *      15.05.2020
 *      Contrôleur Api pour la validation des images utilisateurs
 *      Permet de gérer les photos des utilisateurs (Admin seulement)
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
    public class PicturesAdminController : ControllerBase
    {
        private readonly LoveMirroringContext _context;

        public PicturesAdminController(LoveMirroringContext context)
        {
            _context = context;
        }

        // Récupérer seulement des photos non validées
        // GET: api/PicturesAdmin
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Picture>>> GetPictures()
        {
            return await _context.Pictures.Include(p => p.IdNavigation).Where(p => !p.PictureConfirmed).ToListAsync();
        }

        // GET: api/PicturesAdmin/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Picture>> GetPicture(short id)
        {
            var picture = await _context.Pictures.FindAsync(id);

            if (picture == null)
            {
                return NotFound();
            }

            return picture;
        }

        // PUT: api/PicturesAdmin/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPicture(short id, Picture picture)
        {
            if (id != picture.PictureId)
            {
                return BadRequest();
            }

            _context.Entry(picture).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PictureExists(id))
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

        // DELETE: api/PicturesAdmin/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Picture>> DeletePicture(short id)
        {
            var picture = await _context.Pictures.FindAsync(id);
            if (picture == null)
            {
                return NotFound();
            }

            _context.Pictures.Remove(picture);
            await _context.SaveChangesAsync();

            return picture;
        }

        private bool PictureExists(short id)
        {
            return _context.Pictures.Any(e => e.PictureId == id);
        }
    }
}
