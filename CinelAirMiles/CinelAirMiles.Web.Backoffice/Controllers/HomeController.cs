namespace CinelAirMiles.Web.Backoffice.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using CinelAirMiles.Common.Repositories;
    using CinelAirMiles.Web.Backoffice.Helpers.Interfaces;
    using CinelAirMiles.Web.Backoffice.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    //TODO: News & welcome page
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IClientRepository _clientRepository;
        private readonly IPartnerRepository _partnerRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IContactFormRepository _contactFormRepository;
        private readonly IComplaintRepository _complaintRepository;

        public HomeController(
            IUserHelper userHelper,
            IClientRepository clientRepository,
            IPartnerRepository partnerRepository,
            ISubscriptionRepository subscriptionRepository,
            IContactFormRepository contactFormRepository,
            IComplaintRepository complaintRepository)
        {
            _userHelper = userHelper;
            _clientRepository = clientRepository;
            _partnerRepository = partnerRepository;
            _subscriptionRepository = subscriptionRepository;
            _contactFormRepository = contactFormRepository;
            _complaintRepository = complaintRepository;
        }
        
        
        public async Task<IActionResult> Index()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

            var model = new CountDataViewModel
            {
                EmployeesCount = await _userHelper.GetEmployeesCountAsync(),
                ClientCount = await _clientRepository.GetClientsCountAsync(),
                PartnerCount = await _partnerRepository.GetPartnerCountAsync(),
                BenefitsCount = await _partnerRepository.GetBenefitsCountAsync(),
                SubscriptionsCount = await _subscriptionRepository.GetCountAsync(),
                ContactFormCount = await _contactFormRepository.GetCountAsync(),
                ComplaintsCount = await _complaintRepository.GetComplaintsCountAsync(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return View(model);
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
