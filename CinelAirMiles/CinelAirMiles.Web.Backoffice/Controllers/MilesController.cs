namespace CinelAirMiles.Web.Backoffice.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;
    using CinelAirMiles.Common.Repositories;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using CinelAirMiles.Web.Backoffice.Helpers.Interfaces;
    using CinelAirMiles.Web.Backoffice.Models;
    using Microsoft.AspNetCore.Authorization;

    [Authorize]
    public class MilesController : Controller
    {
        //private readonly ApplicationDbContext _context; //TODO change to repo
        readonly IClientRepository _clientRepository;
        readonly IMileRepository _mileRepository;
        readonly IMilesTypeRepository _milesTypeRepository;
        readonly IConverterHelper _converterHelper;
        readonly ICombosHelper _combosHelper;

        public MilesController(
            //ApplicationDbContext context,
            IClientRepository clientRepository,
            IMileRepository mileRepository,
            IConverterHelper converterHelper,
            IMilesTypeRepository milesTypeRepository,
            ICombosHelper combosHelper)
        {
            //_context = context;
            _clientRepository = clientRepository;
            _mileRepository = mileRepository;
            _converterHelper = converterHelper;
            _milesTypeRepository = milesTypeRepository;
            _combosHelper = combosHelper;
        }

        // GET: Miles
        public async Task<IActionResult> Index()
        {
            //return View(await _context.Miles.ToListAsync());

            return View(await _mileRepository.GetAll().ToListAsync());
        }

        // GET: Miles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //TODO: Non draggable description textarea
            if (id == null)
            {
                return NotFound();
            }

            var mile = await _mileRepository.GetMileWithClientAndTypeAsync(id);

            if (mile == null)
            {
                return NotFound();
            }

            return View(mile);
        }

        // GET: Miles/Create
        public IActionResult Create()
        {
            var model = new CreateMileViewModel
            {
                MilesType = _combosHelper.GetComboMilesTypes()
            };

            return View(model);
        }

        // POST: Miles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var client = await _clientRepository
                    .GetClientByNumberAsync(model.MilesProgramNumber);

                if(client == null)
                {
                    ModelState.AddModelError(string.Empty, "Client does not exist");

                    model.MilesType = _combosHelper.GetComboMilesTypes();

                    return View(model);
                }

                var milesType = await _milesTypeRepository
                    .GetByIdAsync(model.MilesTypeId);

                if (milesType == null)
                {
                    ModelState.AddModelError(string.Empty, "Type does not exist");

                    model.MilesType = _combosHelper.GetComboMilesTypes();

                    return View(model);
                }

                var mile = _converterHelper.CreateMileViewModelToMile(model, client, milesType);

                await _mileRepository.CreateAsync(mile);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Miles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var mile = await _context.Miles.FindAsync(id);

            var mile = await _mileRepository.GetByIdAsync(id.Value);

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
        public async Task<IActionResult> Edit(int id, Mile mile)
        {
            if (id != mile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(mile);
                    //await _context.SaveChangesAsync();

                    await _mileRepository.UpdateAsync(mile);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await MileExistsAsync(mile.Id))
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

            //var mile = await _context.Miles
            //    .FirstOrDefaultAsync(m => m.Id == id);

            var mile = await _mileRepository.GetByIdAsync(id.Value);

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
            //var mile = await _context.Miles.FindAsync(id);
            //_context.Miles.Remove(mile);
            //await _context.SaveChangesAsync();

            var mile = await _mileRepository.GetByIdAsync(id);
            await _mileRepository.DeleteAsync(mile);

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> MileExistsAsync(int id)
        {
            return await _mileRepository.ExistsAsync(id);

            //return _context.Miles.Any(e => e.Id == id);
        }
    }
}
