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
    public class MilesTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MilesTypesController(ApplicationDbContext context) //TODO change to repo
        {
            _context = context;
        }


        // GET: MilesTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.MilesTypes.ToListAsync());
        }


        // GET: MilesTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var milesType = await _context.MilesTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (milesType == null)
            {
                return NotFound();
            }

            return View(milesType);
        }


        // GET: MilesTypes/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: MilesTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MilesType milesType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(milesType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(milesType);
        }


        // GET: MilesTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var milesType = await _context.MilesTypes.FindAsync(id);
            if (milesType == null)
            {
                return NotFound();
            }
            return View(milesType);
        }


        // POST: MilesTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MilesType milesType)
        {
            if (id != milesType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(milesType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MilesTypeExists(milesType.Id))
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
            return View(milesType);
        }

        // GET: MilesTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var milesTypes = await _context.MilesTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (milesTypes == null)
            {
                return NotFound();
            }

            return View(milesTypes);
        }

        // POST: MilesTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var milesTypes = await _context.MilesTypes.FindAsync(id);
            _context.MilesTypes.Remove(milesTypes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MilesTypeExists(int id)
        {
            return _context.MilesTypes.Any(e => e.Id == id);
        }
    }
}
