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
            return await _context.Pictures.ToListAsync();
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
        public string PostPicture(List<IFormFile> files)
        {
            try
            {
                string message = "No files found";

                if (files.Count() > 0)
                {
                    message = "";
                }
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        if (!Directory.Exists(_environnement.WebRootPath + "\\Upload\\"))
                        {
                            Directory.CreateDirectory(_environnement.WebRootPath + "\\Upload\\");
                        }
                        using (FileStream fileStream = System.IO.File.Create(_environnement.WebRootPath + "\\Upload\\" + file.FileName))
                        {
                            file.CopyTo(fileStream);
                            fileStream.Flush();
                            message += "\\Upload\\" + file.FileName + "\n";
                        }
                    }
                }
                return message;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
            //Picture picture = null;
            //_context.Pictures.Add(picture);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetPicture", new { id = picture.PictureId }, picture);
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
