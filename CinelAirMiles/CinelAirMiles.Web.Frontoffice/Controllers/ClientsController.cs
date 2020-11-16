using CinelAirMiles.Common.Entities;
using CinelAirMiles.Common.Models;
using CinelAirMiles.Common.Repositories;
using CinelAirMiles.Web.Frontoffice.Helpers.Interfaces;
using CinelAirMiles.Web.Frontoffice.Models;
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
        readonly IProgramTierRepository _programTierRepository;
        private readonly IComplaintRepository _complaintRepository;

        public ClientsController(
            IClientRepository clientRepository,
            IMileRepository mileRepository,
            IConverterHelper converterHelper,
            IUserHelper userHelper,
            IProgramTierRepository programTierRepository,
            IComplaintRepository complaintRepository)
        {
            _converterHelper = converterHelper;
            _mileRepository = mileRepository;
            _clientRepository = clientRepository;
            _userHelper = userHelper;
            _programTierRepository = programTierRepository;
            _complaintRepository = complaintRepository;
        }

        public async Task<IActionResult> MyAccount()
        {
            var client = await _clientRepository.GetClientByEmailAsync(this.User.Identity.Name);

            return View(client);
        }

        //public async Task<IActionResult> MyStatus()
        //{
        //    return View();
        //}

        public IActionResult Complaints()
        {
            var model = new Complaint();
            return View(model);
        }

        public async Task<IActionResult> ComplaintsList()
        {
            var client = await _clientRepository.GetClientByEmailAsync(User.Identity.Name);

            if (client == null)
            {
                return NotFound();
            }

            var list = await _complaintRepository.GetComplaintAssociatedWithUserAsync(client.MilesProgramNumber);

            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Complaints(Complaint model)
        {
            if (ModelState.IsValid)
            {
                var client = await _clientRepository.GetClientByEmailAsync(User.Identity.Name);

                if (client == null)
                {
                    return NotFound();
                }

                DateTime today = DateTime.Today;

                model.MilesProgramNumber = client.MilesProgramNumber;
                model.ComplaintDate = today;
              
                await _complaintRepository.CreateComplaintAsync(model, client.User);
                ViewData["Message"] = "Complain submitted succesfully!";
                return View();
            }

            return RedirectToAction(nameof(Complaints));
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

            var bonus = miles.Where(m => m.MilesType.Description == "Bonus");

            var model = _converterHelper.FromMileToMilesViewModel(bonus);

            return View(model);
        }

        public async Task<IActionResult> ManageGoldReference()
        {
            var client = await _clientRepository.GetClientByEmailAsync(User.Identity.Name);

            if(client == null)
            {
                return NotFound();
            }

            var tier = await _programTierRepository.GetByDescriptionAsync("Gold");

            if (tier == null)
            {
                return NotFound();
            }

            if (client.ProgramTier != tier)
            {
                return RedirectToAction(nameof(MyAccount));
            }

            var model = new GoldReferenceViewModel();

            if (client.IsInReferrerProgram)
            {
                var userIsAlreadyReferred = await _clientRepository.CheckIfClientIsAlreadyReferredAsync(client);

                if (userIsAlreadyReferred)
                {
                    model.ReferrerClientNumber = await _clientRepository.GetReferrerClientNumber(client);
                    model.ReferredClientNumber = client.MilesProgramNumber;
                    model.ViewState = 0;
                    model.Info = $"The client with number {model.ReferrerClientNumber} is sharing their Gold tier with you";

                    return View(model);
                }

                model.ReferrerClientNumber = client.MilesProgramNumber;
                model.ReferredClientNumber = await _clientRepository.GetReferredClientNumber(client);
                model.ViewState = 1;
                model.Info = $"You are sharing your Gold status with the client number {model.ReferredClientNumber}";

                return View(model);
            }

            model.ReferrerClientNumber = client.MilesProgramNumber;
            model.ViewState = 2;
            model.Info = "Insert the client number with which you wish to share your Gold tier with";

            return View(model);
        }

        //TODO Finish the post
        //TODO If in the razor to hide the Manage gold button in the layout
        [HttpPost]
        public async Task<IActionResult> ManageGoldReference(GoldReferenceViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                string message;

                if (model.ViewState == 0)
                {
                    return RedirectToAction(nameof(MyAccount));
                }
                else if (model.ViewState == 1)
                {
                    message = await _clientRepository.RemoveReferenceClientsAsync(model.ReferrerClientNumber, model.ReferredClientNumber);

                    model.ViewState = 2;
                    ViewData["Message"] = message;
                    return View(model);
                }

                var clientNumberToBeReferred = await _clientRepository.GetClientByNumberAsync(model.ReferredClientNumber);

                if(clientNumberToBeReferred == null)
                {
                    ViewData["Message"] = "A client with this number was not found";
                    return View(model);
                }

                message = await _clientRepository.AddClientsToReferenceProgramAsync(model.ReferrerClientNumber, model.ReferredClientNumber);

                model.ViewState = 1;
                ViewData["Message"] = message;
                return View(model);
            }

            return View(model);
        }

        public IActionResult ManageMiles()
        {
            return View();
        }
    }
}
