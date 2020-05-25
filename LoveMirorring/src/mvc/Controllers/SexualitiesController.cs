using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvc.Models;

namespace mvc.Controllers
{
    public class SexualitiesController : Controller
    {
        private readonly LoveMirroringContext _context;

        public SexualitiesController(LoveMirroringContext context)
        {
            _context = context;
        }

        // GET: Sexualities
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sexualities.ToListAsync());
        }

        // GET: Sexualities/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sexuality = await _context.Sexualities
                .FirstOrDefaultAsync(m => m.SexualityId == id);
            if (sexuality == null)
            {
                return NotFound();
            }

            return View(sexuality);
        }

        // GET: Sexualities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sexualities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SexualityId,SexualityName")] Sexuality sexuality)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sexuality);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sexuality);
        }

        // GET: Sexualities/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sexuality = await _context.Sexualities.FindAsync(id);
            if (sexuality == null)
            {
                return NotFound();
            }
            return View(sexuality);
        }

        // POST: Sexualities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("SexualityId,SexualityName")] Sexuality sexuality)
        {
            if (id != sexuality.SexualityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sexuality);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SexualityExists(sexuality.SexualityId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sexuality);
        }

        // GET: Sexualities/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sexuality = await _context.Sexualities
                .FirstOrDefaultAsync(m => m.SexualityId == id);
            if (sexuality == null)
            {
                return NotFound();
            }

            return View(sexuality);
        }

        // POST: Sexualities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var sexuality = await _context.Sexualities.FindAsync(id);
            _context.Sexualities.Remove(sexuality);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SexualityExists(short id)
        {
            return _context.Sexualities.Any(e => e.SexualityId == id);
        }
    }
}
