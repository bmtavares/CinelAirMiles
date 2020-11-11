using CinelAirMiles.Common.Repositories;
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

        public ClientsController(
            IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<IActionResult> MyAccount(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _clientRepository.GetClientWithDetailsAsync(id.Value);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }
    }
}
