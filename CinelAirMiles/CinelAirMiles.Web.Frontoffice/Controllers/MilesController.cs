using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinelAirMiles.Common.Entities;
using CinelAirMiles.Common.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CinelAirMiles.Web.Frontoffice.Controllers
{
    public class MilesController : Controller
    {
        readonly IClientRepository _clientRepository;
        readonly IMileRepository _mileRepository;
        readonly IMilesTransactionRepository _milesTransactionRepository;

        public MilesController(
            IMileRepository mileRepository,
            IMilesTransactionRepository milesTransactionRepository,
            IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
            _mileRepository = mileRepository;
            _milesTransactionRepository = milesTransactionRepository;
        }

        public async Task<IActionResult> Index()
        {
            var client = await _clientRepository.GetClientByEmailAsync(User.Identity.Name);

            if(client == null)
            {
                return NotFound();
            }

            var miles = await _mileRepository.GetMilesAssociatedWithClientAsync(client.Id);

            if(miles == null)
            {
                return NotFound();
            }

            return View(miles);
        }

        public async Task<IActionResult> PurchaseMiles()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PurchaseMiles(int? id)
        {
            //await _mileRepository.PurchaseMilesAsync(id.Value);

            //Placeholder
            return Ok();
        }

        public async Task<IActionResult> ExtendMiles()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ExtendMiles(int? id)
        {
            //Placeholder
            return Ok();
        }

        public async Task<IActionResult> TransferMiles()
        {
            //TODO Probably gonna need to create a TransferMilesViewModel
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TransferMiles(int? clientId)
        {
            var client = await _clientRepository.GetClientByEmailAsync(User.Identity.Name);

            if (client == null)
            {
                return NotFound();
            }

            //await _milesTransactionRepository.TransferMilesToAnotherClientAsync(client, clientId.Value);

            //Placeholder
            return Ok();
        }

        public async Task<IActionResult> ConvertMiles()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConvertMiles(int? id)
        {
            //Placeholder
            return Ok();
        }
    }
}
