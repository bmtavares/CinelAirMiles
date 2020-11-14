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
using Syncfusion.EJ2.Navigations;

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

        public IActionResult PurchaseMiles()
        {
            var model = new PurchaseMilesViewModel
            {
                Quantity = 2000
            };

            model.ValueToPay = Math.Round(model.Quantity * 0.035m, 0);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PurchaseMiles(PurchaseMilesViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Quantity != 2000)
                {
                    ModelState.AddModelError(string.Empty, "Can only purchase 2000 miles per transaction");
                }

                var client = await _clientRepository.GetClientByEmailAsync(User.Identity.Name);

                if (client == null)
                {
                    return NotFound();
                }

                model.CreditCardInfo.Client = client;

                await _creditCardRepository.CheckExistingCreditCardByNumberAsync(model.CreditCardInfo);

                ViewData["Message"] = await _milesTransactionRepository.PurchaseMilesAsync(model.Quantity, client);

                ResetModel(model);

                return View(model);
            }

            return View(model);
        }


        // -------------------------------------------TEMPORARY-------------------------------------------
        //public async Task<IActionResult> PurchaseMiles2()
        //{
        //    var client = await _clientRepository.GetClientByEmailAsync(User.Identity.Name);

        //    var creditCards = _creditCardRepository.GetCreditCardsAssociatedWithClient(client);

        //    var model = new PurchaseMilesViewModel
        //    {
        //        CreditCards = _combosHelper.GetCreditCards(creditCards)
        //    };

        //    ViewBag.NewCardHeader = new TabHeader { Text = "Register a new credit card" };
        //    ViewBag.ExistingCardsHeader = new TabHeader { Text = "Use an existing credit card" };

        //    return View(model);
        //}

        //[HttpPost]
        //public async Task<IActionResult> PurchaseMiles2(PurchaseMilesViewModel model)
        //{
        //    if (model.Quantity > 2000)
        //    {
        //        ModelState.AddModelError(string.Empty, "Can only purchase up to 2000 miles per transaction");
        //    }

        //    var client = await _clientRepository.GetClientByEmailAsync(User.Identity.Name);

        //    if (client == null)
        //    {
        //        return NotFound();
        //    }



        //    if (model.CreditCardInfoId == 0)
        //    {
        //        model.CreditCardInfo = await _creditCardRepository.CheckExistingCreditCardByNumberAsync(model.CreditCardInfo);
        //    }
        //    else
        //    {
        //        var creditCard = await _creditCardRepository.GetByIdAsync(model.CreditCardInfoId);
        //    }

        //    await _milesTransactionRepository.PurchaseMilesAsync(model.Quantity, client, creditCard);

        //    return View();
        //}
        // -------------------------------------------TEMPORARY-------------------------------------------


        public IActionResult TransferMiles()
        {
            var model = new TransferMilesViewModel
            {
                Quantity = 2000
            };

            model.ValueToPay = Math.Round(model.Quantity * 0.035m, 0);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> TransferMiles(TransferMilesViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Quantity != 2000)
                {
                    ModelState.AddModelError(string.Empty, "Can only transfer 2000 miles per transaction");
                }

                var client = await _clientRepository.GetClientByEmailAsync(User.Identity.Name);

                if (client == null)
                {
                    return NotFound();
                }

                var receivingClient = await _clientRepository.GetClientByNumberAsync(model.ClientToTransferToNumber);

                if (receivingClient == null)
                {
                    ModelState.AddModelError(model.ClientToTransferToNumber, "Client number does not exist");
                }

                model.CreditCardInfo.Client = client;

                await _creditCardRepository.CheckExistingCreditCardByNumberAsync(model.CreditCardInfo);

                ViewData["Message"] = await _milesTransactionRepository.TransferMilesAsync(model.Quantity, receivingClient, client);

                ResetModel(model);

                return View(model);
            }

            return View(model);
        }

        public IActionResult ConvertMiles()
        {
            var model = new ConvertMilesViewModel
            {
                Quantity = 2000
            };

            model.ValueToPay = Math.Round(model.Quantity * 0.035m, 0);
            model.StatusMilesToReceive = model.Quantity / 2;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ConvertMiles(ConvertMilesViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Quantity != 2000)
                {
                    ModelState.AddModelError(string.Empty, "Can only convert 2000 miles per transaction");
                }

                var client = await _clientRepository.GetClientByEmailAsync(User.Identity.Name);

                if (client == null)
                {
                    return NotFound();
                }

                model.CreditCardInfo.Client = client;

                await _creditCardRepository.CheckExistingCreditCardByNumberAsync(model.CreditCardInfo);

                ViewData["Message"] = await _milesTransactionRepository.ConvertMilesAsync(model.Quantity, client);

                ResetModel(model);

                return View(model);
            }

            return View(model);
        }

        //public async Task<IActionResult> ExtendMiles()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> ExtendMiles(int? id)
        //{

        //}

        void ResetModel(PurchaseMilesViewModel model)
        {
            model.Quantity = 2000;
            model.ValueToPay = Math.Round(model.Quantity * 0.035m, 0);
            model.CreditCardInfo.FirstName = string.Empty;
            model.CreditCardInfo.LastName = string.Empty;
            model.CreditCardInfo.Number = string.Empty;
            model.CreditCardInfo.CVC = string.Empty;
            model.CreditCardInfo.Month = null;
            model.CreditCardInfo.Year = null;
        }
    }
}
