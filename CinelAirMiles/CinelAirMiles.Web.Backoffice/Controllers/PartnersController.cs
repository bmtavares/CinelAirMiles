using CinelAirMiles.Common.Data;
using CinelAirMiles.Common.Entities;
using CinelAirMiles.Common.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinelAirMiles.Web.Backoffice.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class PartnersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPartnerRepository _partnerRepository;

        public PartnersController(
            ApplicationDbContext context,
            IPartnerRepository partnerRepository)
        {
            _context = context;
            _partnerRepository = partnerRepository;
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Partner partner)
        {
            if (ModelState.IsValid)
            {
                await _partnerRepository.CreateAsync(partner);

                return RedirectToAction(nameof(Index));
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    if (!await PartnersExistsAsync(partner.Id))
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

        private async Task<bool> PartnersExistsAsync(int id)
        {
            return await _partnerRepository.ExistsAsync(id);
        }
    }
}
