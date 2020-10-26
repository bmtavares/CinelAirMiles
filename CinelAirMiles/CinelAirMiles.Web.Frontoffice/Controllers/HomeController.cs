using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CinelAirMiles.Web.Frontoffice.Models;
using CinelAirMiles.Web.Frontoffice.Helpers.Interfaces;

namespace CinelAirMiles.Web.Frontoffice.Controllers
{
    public class HomeController : Controller
    {
        readonly IXmlHelper _xmlHelper;

        public HomeController(
            IXmlHelper xmlHelper)
        {
            _xmlHelper = xmlHelper;
        }

        public IActionResult Index()
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

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
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
