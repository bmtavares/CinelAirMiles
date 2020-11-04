namespace CinelAirMiles.Web.Backoffice.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;
    using CinelAirMiles.Common.Repositories;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Authorize]
    public class MilesTypesController : Controller
    {
        //readonly ApplicationDbContext _context;
        readonly IMilesTypeRepository _milesTypeRepository;

        public MilesTypesController(
            //ApplicationDbContext context,
            IMilesTypeRepository milesTypeRepository)
        {
            //_context = context;
            _milesTypeRepository = milesTypeRepository;
        }


        // GET: MilesTypes
        public async Task<IActionResult> Index()
        {
            //return View(await _context.MilesTypes.ToListAsync());
            return View(await _milesTypeRepository.GetAll().ToListAsync());

        }


        // GET: MilesTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var milesType = await _context.MilesTypes
            //    .FirstOrDefaultAsync(m => m.Id == id);

            var milesType = await _milesTypeRepository.GetByIdAsync(id.Value);

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

                //_context.Add(milesType);
                //await _context.SaveChangesAsync();

                await _milesTypeRepository.CreateAsync(milesType);

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

            //var milesType = await _context.MilesTypes.FindAsync(id);

            var milesType = await _milesTypeRepository.GetByIdAsync(id.Value);

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
                    //_context.Update(milesType);
                    //await _context.SaveChangesAsync();

                    await _milesTypeRepository.UpdateAsync(milesType);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await MilesTypeExistsAsync(milesType.Id))
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

            //var milesTypes = await _context.MilesTypes
            //    .FirstOrDefaultAsync(m => m.Id == id);

            var milesType = await _milesTypeRepository.GetByIdAsync(id.Value);

            if (milesType == null)
            {
                return NotFound();
            }

            return View(milesType);
        }

        // POST: MilesTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var milesTypes = await _context.MilesTypes.FindAsync(id);
            //_context.MilesTypes.Remove(milesTypes);
            //await _context.SaveChangesAsync();

            var milesType = await _milesTypeRepository.GetByIdAsync(id);
            await _milesTypeRepository.DeleteAsync(milesType);

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> MilesTypeExistsAsync(int id)
        {
            //return _context.MilesTypes.Any(e => e.Id == id);

            return await _milesTypeRepository.ExistsAsync(id);
        }
    }
}
