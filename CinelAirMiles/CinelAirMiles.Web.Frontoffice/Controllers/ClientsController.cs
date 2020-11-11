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
        private readonly IUserHelper _userHelper;

        public ClientsController(
            IClientRepository clientRepository,
            IUserHelper userHelper)
        {
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
            return View();
        }

        public async Task<IActionResult> ManageMiles()
        {
            return View();
        }
    }
}
