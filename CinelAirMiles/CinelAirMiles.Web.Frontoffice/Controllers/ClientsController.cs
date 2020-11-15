using CinelAirMiles.Common.Models;
using CinelAirMiles.Common.Repositories;
using CinelAirMiles.Web.Frontoffice.Helpers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinelAirMiles.Web.Frontoffice.Controllers
{
    public class ClientsController : Controller
    {
        readonly IClientRepository _clientRepository;
        readonly IConverterHelper _converterHelper;
        readonly IMileRepository _mileRepository;
        private readonly IUserHelper _userHelper;

        public ClientsController(
            IClientRepository clientRepository,
            IMileRepository mileRepository,
            IConverterHelper converterHelper,
            IUserHelper userHelper)
        {
            _converterHelper = converterHelper;
            _mileRepository = mileRepository;
            _clientRepository = clientRepository;
            _userHelper = userHelper;
        }

        public async Task<IActionResult> MyAccount()
        {
            var client = await _clientRepository.GetClientByEmailAsync(this.User.Identity.Name);

            return View(client);
        }

        public async Task<IActionResult> MyStatus()
        {
            return View();
        }

        public async Task<IActionResult> MyBalance()
        {
            var client = await _clientRepository.GetClientByEmailAsync(User.Identity.Name);

            if (client == null)
            {
                return NotFound();
            }

            var miles = await _mileRepository.GetMilesAssociatedWithClientAsync(client.Id);

            if (miles == null)
            {
                return NotFound();
            }

            ViewData["StatusBalance"] = miles.Where(m => m.MilesType.Description == "Status").Sum(m => m.Balance);
            ViewData["BonusBalance"] = miles.Where(m => m.MilesType.Description == "Bonus").Sum(m => m.Balance);

            var model = _converterHelper.FromMileToMilesViewModel(miles);

            return View(model);
        }

        public async Task<IActionResult> ManageMiles()
        {
            return View();
        }
    }
}
