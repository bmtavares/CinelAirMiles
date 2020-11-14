namespace CinelAirMiles.Web.Backoffice.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using CinelAirMiles.Web.Backoffice.Helpers.Interfaces;
    using CinelAirMiles.Web.Backoffice.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    //TODO: News & welcome page
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserHelper _userHelper;

        public HomeController(IUserHelper userHelper)
        {
            _userHelper = userHelper;
        }
        
        
        public async Task<IActionResult> Index()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            return View(user);
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
