using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinelAirMiles.Common.Entities;
using CinelAirMiles.Common.Models;
using CinelAirMiles.Common.Repositories;
using CinelAirMiles.Web.Frontoffice.Helpers.Interfaces;
using CinelAirMiles.Web.Frontoffice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace CinelAirMiles.Web.Frontoffice.Controllers
{
    public class MilesController : Controller
    {
        readonly IClientRepository _clientRepository;
        readonly IMileRepository _mileRepository;
        readonly IMilesTransactionRepository _milesTransactionRepository;
        readonly ICreditCardRepository _creditCardRepository;
        readonly ICombosHelper _combosHelper;

        public MilesController(
            IMileRepository mileRepository,
            IMilesTransactionRepository milesTransactionRepository,
            IClientRepository clientRepository,
            ICreditCardRepository creditCardRepository,
            ICombosHelper combosHelper)
        {
            _clientRepository = clientRepository;
            _mileRepository = mileRepository;
            _milesTransactionRepository = milesTransactionRepository;
            _creditCardRepository = creditCardRepository;
            _combosHelper = combosHelper;
        }

        public async Task<IActionResult> PurchaseMiles()
        {
            //var client = await _clientRepository.GetClientByEmailAsync(User.Identity.Name);

            //var creditCards = _creditCardRepository.GetCreditCardsAssociatedWithClient(client);

            //var model = new PurchaseMilesViewModel
            //{
            //    CreditCards = _combosHelper.GetCreditCards(creditCards)
            //};

            var model = new PurchaseMilesViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PurchaseMiles(PurchaseMilesViewModel model)
        {
            if(model.Quantity > 2000)
            {
                ModelState.AddModelError(string.Empty, "Can only purchase up to 2000 miles per transaction");
            }

            var client = await _clientRepository.GetClientByEmailAsync(User.Identity.Name);

            if(client == null)
            {
                return NotFound();
            }

            var creditCard = await _creditCardRepository.CheckExistingCreditCardByNumberAsync(model.CreditCardInfo);

            await _milesTransactionRepository.PurchaseMilesAsync(model.Quantity, client, creditCard);

            return View();
        }

        public async Task<IActionResult> ExtendMiles()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> ExtendMiles(int? id)
        //{

        //}

        public async Task<IActionResult> TransferMiles()
        {

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

        //[HttpPost]
        //public async Task<IActionResult> ConvertMiles(int? id)
        //{

        //}
    }
}
