/*
 *      Auteur : Tim Allemann
 *      2020.04.27
 *      Permet de gérer les images des profils de l'utilisateur
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;
using Api.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Unosquare.Swan;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        private readonly LoveMirroringContext _context;
        public static IWebHostEnvironment _environnement;

        public PicturesController(LoveMirroringContext context,
                                  IWebHostEnvironment environnement)
        {
            _context = context;
            _environnement = environnement;
        }

        // GET: api/Pictures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Picture>>> GetPictures()
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

            return await _context.Pictures.Where(p => p.Id == id).ToListAsync();
        }

        // GET: api/Pictures/5
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

        // PUT: api/Pictures/5
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

        public class FilUploadAPI
        {
            public IFormFile files { get; set; }
        }

        // POST: api/Pictures
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<IActionResult> PostPicture(List<IFormFile> files)
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

            string userName = await _context.AspNetUsers.Where(a => a.Id == id).Select(a => a.UserName).SingleOrDefaultAsync();
            try
            {
                string folder = "Upload";
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        if (!Directory.Exists(Path.Combine(_environnement.WebRootPath, folder)))
                        {
                            Directory.CreateDirectory(Path.Combine(_environnement.WebRootPath, folder));
                        }

                        string filename = userName + DateTime.Now.ToString("_yyyy-MM-dd_HH-mm-ss") + file.FileName;
                        using (FileStream fileStream = System.IO.File.Create(Path.Combine(_environnement.WebRootPath, folder, filename)))
                        {
                            file.CopyTo(fileStream);
                            fileStream.Flush();
                            _context.Pictures.Add(new Picture { Id = id, PictureView = folder + "/" + filename });
                            _context.SaveChanges();
                        }
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // DELETE: api/Pictures/5
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
