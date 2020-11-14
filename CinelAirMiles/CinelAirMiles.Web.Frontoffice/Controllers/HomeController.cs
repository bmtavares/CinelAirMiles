using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CinelAirMiles.Web.Frontoffice.Models;
using CinelAirMiles.Web.Frontoffice.Helpers.Interfaces;
using CinelAirMiles.Common.Repositories;
using CinelAirMiles.Common.Entities;

namespace CinelAirMiles.Web.Frontoffice.Controllers
{
    public class HomeController : Controller
    {
        readonly IXmlHelper _xmlHelper;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IContactFormRepository _contactFormRepository;

        public HomeController(
            IXmlHelper xmlHelper,
            ISubscriptionRepository subscriptionRepository,
            IContactFormRepository contactFormRepository)
        {
            _xmlHelper = xmlHelper;
            _subscriptionRepository = subscriptionRepository;
            _contactFormRepository = contactFormRepository;
        }

        public IActionResult Index()
        {
            ViewData["Message"] = "Index";
            return View();
            
        }

        public IActionResult News()
        {
            NewsViewModel news;

            try
            {
                news = _xmlHelper.DeserializeNewsXml("https://www.flightglobal.com/127.fullrss");
            }
            catch
            {
                news = new NewsViewModel
                {
                    Channel = new NewsChannelViewModel
                    {
                        Item = new List<NewsItemViewModel>
                        {
                            new NewsItemViewModel
                            {
                                Title = "The news couldn't be displayed due to an internal error!\nContact the administrator"
                            }
                        }
                    }
                };
            }

            return View(news);
        }

        [HttpPost]
        public async Task<IActionResult> AddSubscriptionAsync(Subscription subscription)
        {
            if (ModelState.IsValid)
            {
                await _subscriptionRepository.CreateAsync(subscription);
                this.ViewBag.Subscription = "Subscription succesfully";                
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            var model = new ContactForm();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactForm model)
        {
            if (ModelState.IsValid)
            {
                await _contactFormRepository.CreateAsync(model);
                ViewBag.ContactForm = "Subscription succesfully";
            }
                       
            return RedirectToAction(nameof(Contact));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
