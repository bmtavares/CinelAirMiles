namespace CinelAirMiles.Web.Backoffice.Controllers
{
    using System.Threading.Tasks;

    using CinelAirMiles.Common.Entities;
    using CinelAirMiles.Common.Repositories;
    using CinelAirMiles.Web.Backoffice.Helpers.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Authorize(Roles = "Admin, User")]
    public class PartnersController : Controller
    {
        private readonly IPartnerRepository _partnerRepository;
        private readonly IUserHelper _userHelper;

        public PartnersController(
            IPartnerRepository partnerRepository,
            IUserHelper userHelper)
        {
            _partnerRepository = partnerRepository;
            _userHelper = userHelper;
        }

        // GET: Partners
        public async Task<IActionResult> Index()
        {
            return View(await _partnerRepository.GetAll().ToListAsync());
        }


        // GET: Partners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mile = await _partnerRepository.GetByIdAsync(id.Value);

            if (mile == null)
            {
                return NotFound();
            }

            return View(mile);
        }


        // GET: Partners/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Partners/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Partner partner)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                var message = await _partnerRepository.CreatePartnerAsync(partner, currentUser);

                ViewData["Message"] = message;

                return View(partner);
            }
            return View(partner);
        }


        // GET: Partners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var milesType = await _partnerRepository.GetByIdAsync(id.Value);

            if (milesType == null)
            {
                return NotFound();
            }

            return View(milesType);
        }


        // POST: Partners/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Partner partner)
        {
            if (id != partner.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _partnerRepository.UpdateAsync(partner);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _partnerRepository.ExistsAsync(partner.Id))
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
            return View(partner);
        }

        // GET: Partners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partner = await _partnerRepository.GetByIdAsync(id.Value);

            if (partner == null)
            {
                return NotFound();
            }

            return View(partner);
        }

        // POST: Partners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var partner = await _partnerRepository.GetByIdAsync(id);
            await _partnerRepository.DeleteAsync(partner);

            return RedirectToAction(nameof(Index));
        }
    }
}
