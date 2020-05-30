/*
 * Auteur : Sébastien Berger 
 * Date : 29.05.2020
 * Description : Contrôleur pour le traitement des publicités 
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
using System.IO;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Api.ViewModels;

namespace Api.Controllers
{
    [Authorize(Policy = "Administrateur")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdsController : ControllerBase
    {
        private readonly LoveMirroringContext _context;
        private IConfiguration Configuration { get; set; }
        public static IWebHostEnvironment _environnement;

        public AdsController(LoveMirroringContext context, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _context = context;
            Configuration = configuration;
            _environnement = environment;
        }

        // GET: api/Ads
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ad>>> GetAds()
        {
            return await _context.Ads.ToListAsync();
        }

        // GET: api/Ads/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ad>> GetAd(short id)
        {
            var ad = await _context.Ads.FindAsync(id);

            if (ad == null)
            {
                return NotFound();
            }

            return ad;
        }

        // PUT: api/Ads/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAd(short id, AdPost adPost)
        {
            if (id != adPost.Id)
            {
                return BadRequest();
            }

            string folder = "Ads";

            AdInput ad = new AdInput();
            ad.Id = adPost.Id;
            ad.Titre = adPost.Titre;
            ad.Description = adPost.Description;
            ad.Link = adPost.Link;

            byte[] data = Convert.FromBase64String(adPost.file);
            var stream = new MemoryStream(data);
            IFormFile file = new FormFile(stream, 0, data.Length, adPost.name, adPost.fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = adPost.ContentType,
                ContentDisposition = adPost.ContentDisposition
            };

            ad.file = file;

            if (ad.file.Length > 0)
            {
                if (!Directory.Exists(Path.Combine(_environnement.WebRootPath, folder)))
                {
                    Directory.CreateDirectory(Path.Combine(_environnement.WebRootPath, folder));
                }

                string filename = "_ad" + DateTime.Now.ToString("_yyyy-MM-dd_HH-mm-ss_") + ad.file.FileName;
                using (FileStream fileStream = System.IO.File.Create(Path.Combine(_environnement.WebRootPath, folder, filename)))
                {
                    ad.file.CopyTo(fileStream);
                    fileStream.Flush();
                    _context.Entry(new Ad { Id = ad.Id, Titre = ad.Titre, Description = ad.Description, AdView = folder + "/" + filename, Link = ad.Link }).State = EntityState.Modified;
                }

            }
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdExists(id))
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

        // POST: api/Ads
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        // Description : Permet de décoder l'image et de l'enregistrer dans l'API
        [HttpPost]
        public async Task<ActionResult<Ad>> PostAd(AdPost adPost)
        {
            string folder = "Ads";

            AdInput ad = new AdInput();
            ad.Id = adPost.Id;
            ad.Titre = adPost.Titre;
            ad.Description = adPost.Description;
            ad.Link = adPost.Link;

            byte[] data = Convert.FromBase64String(adPost.file);
            var stream = new MemoryStream(data);
            IFormFile file = new FormFile(stream, 0, data.Length, adPost.name, adPost.fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = adPost.ContentType,
                ContentDisposition = adPost.ContentDisposition
            };

            ad.file = file;

            if (ad.file.Length > 0)
            {
                if (!Directory.Exists(Path.Combine(_environnement.WebRootPath, folder)))
                {
                    Directory.CreateDirectory(Path.Combine(_environnement.WebRootPath, folder));
                }

                string filename = "_ad" + DateTime.Now.ToString("_yyyy-MM-dd_HH-mm-ss_") + ad.file.FileName;
                using (FileStream fileStream = System.IO.File.Create(Path.Combine(_environnement.WebRootPath, folder, filename)))
                {
                    ad.file.CopyTo(fileStream);
                    fileStream.Flush();
                    _context.Ads.Add(new Ad { Id = ad.Id, Titre = ad.Titre, Description = ad.Description, AdView = folder + "/" + filename, Link = ad.Link });
                    _context.SaveChanges();
                }

                await _context.SaveChangesAsync();

            }

            return CreatedAtAction("GetAd", new { id = ad.Id }, ad);
        }

        // DELETE: api/Ads/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Ad>> DeleteAd(short id)
        {
            var ad = await _context.Ads.FindAsync(id);
            if (ad == null)
            {
                return NotFound();
            }

            _context.Ads.Remove(ad);
            await _context.SaveChangesAsync();

            return ad;
        }

        private bool AdExists(short id)
        {
            return _context.Ads.Any(e => e.Id == id);
        }
    }
}
