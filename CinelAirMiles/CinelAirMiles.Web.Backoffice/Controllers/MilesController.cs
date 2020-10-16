using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinelAirMiles.Web.Backoffice.Data;
using CinelAirMiles.Web.Backoffice.Data.Entities;

namespace CinelAirMiles.Web.Backoffice.Controllers
{
    public class MilesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MilesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Miles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Miles.ToListAsync());
        }

        // GET: Miles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mile = await _context.Miles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mile == null)
            {
                return NotFound();
            }

            return View(mile);
        }

        // GET: Miles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Miles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Miles,CreditDate,ExpiryDate")] Mile mile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mile);
        }

        // GET: Miles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mile = await _context.Miles.FindAsync(id);
            if (mile == null)
            {
                return NotFound();
            }
            return View(mile);
        }

        // POST: Miles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Miles,CreditDate,ExpiryDate")] Mile mile)
        {
            if (id != mile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MileExists(mile.Id))
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
            return View(mile);
        }

        // GET: Miles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mile = await _context.Miles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mile == null)
            {
                return NotFound();
            }

            return View(mile);
        }

        // POST: Miles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mile = await _context.Miles.FindAsync(id);
            _context.Miles.Remove(mile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MileExists(int id)
        {
            return _context.Miles.Any(e => e.Id == id);
        }
    }
}
