namespace CinelAirMiles.Web.Backoffice.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Authorize]
    public class ProgramTiersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProgramTiersController(ApplicationDbContext context) //TODO change to repo
        {
            _context = context;
        }


        // GET: ProgramTiers
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProgramTiers.ToListAsync());
        }


        // GET: ProgramTiers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programTier = await _context.ProgramTiers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (programTier == null)
            {
                return NotFound();
            }

            return View(programTier);
        }


        // GET: ProgramTiers/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: ProgramTiers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProgramTier programTier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(programTier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(programTier);
        }


        // GET: ProgramTiers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programTier = await _context.ProgramTiers.FindAsync(id);
            if (programTier == null)
            {
                return NotFound();
            }
            return View(programTier);
        }


        // POST: ProgramTiers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProgramTier programTier)
        {
            if (id != programTier.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(programTier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgramTiersExists(programTier.Id))
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
            return View(programTier);
        }

        // GET: ProgramTiers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programTier = await _context.ProgramTiers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (programTier == null)
            {
                return NotFound();
            }

            return View(programTier);
        }

        // POST: ProgramTiers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var programTier = await _context.ProgramTiers.FindAsync(id);
            _context.ProgramTiers.Remove(programTier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool ProgramTiersExists(int id)
        {
            return _context.ProgramTiers.Any(e => e.Id == id);
        }
    }
}
