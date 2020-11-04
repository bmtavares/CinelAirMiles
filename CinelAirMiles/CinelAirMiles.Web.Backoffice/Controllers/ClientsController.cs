namespace CinelAirMiles.Web.Backoffice.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;
    using CinelAirMiles.Common.Repositories;
    using CinelAirMiles.Web.Backoffice.Helpers.Interfaces;
    using CinelAirMiles.Web.Backoffice.Models;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;

    [Authorize(Roles = "Admin")]
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IClientRepository _clientRepository;
        private readonly ICombosHelper _combosHelper;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;

        public ClientsController(
            ApplicationDbContext context,
            IClientRepository clientRepository,
            ICombosHelper combosHelper,
            IUserHelper userHelper,
            IConverterHelper converterHelper)
        {
            _context = context;
            _clientRepository = clientRepository;
            _combosHelper = combosHelper;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
        }

        // GET: Clients
        public IActionResult Index()
        {
            return View(_clientRepository.GetClientsWithUsers());
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? clientId)
        {
            if (clientId == null)
            {
                return NotFound();
            }

            var client = await _clientRepository.GetClientWithDetailsAsync(clientId.Value);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            var model = new CreateClientViewModel()
            {
                //ProgramTiers = _combosHelper.GetProgramTiers()
            };

            return View(model);
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateClientViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);

                if (user == null)
                {
                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Username,
                        UserName = model.Username,
                        PhoneNumber = model.PhoneNumber,
                        RequirePasswordChange = true,
                        MainRole = "Client"
                    };

                    var result = await _userHelper.AddUserAsync(user, model.Password);

                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user could not be created.");
                        //model.ProgramTiers = _combosHelper.GetProgramTiers();
                        return View(model);
                    }

                    var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    // TODO: Confirm with human interaction
                    await _userHelper.ConfirmEmailAsync(user, token);

                    user = await _userHelper.GetUserByEmailAsync(user.Email);

                    await _clientRepository.CreateClientWithUserAsync(user);

                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, "The username is already taken.");

            }
            //model.ProgramTiers = _combosHelper.GetProgramTiers();
            return View(model);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? clientId)
        {
            if (clientId == null)
            {
                return NotFound();
            }

            var client = await _clientRepository.GetClientWithDetailsAsync(clientId.Value);
            if (client == null)
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByEmailAsync(client.User.Email);
            if (user == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ClientToEditClientViewModel(user, client);
            model.ProgramTiers = _combosHelper.GetProgramTiers();

            return View(model);
        }

        // POST: Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditClientViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByIdAsync(model.Id);
                if(user != null)
                {
                    var client = await _clientRepository.GetClientWithDetailsAsync(model.ClientId);
                    if(client != null)
                    {
                        if(client.User.Id == user.Id)
                        {
                            user.FirstName = model.FirstName;
                            user.LastName = model.LastName;
                            user.PhoneNumber = model.PhoneNumber;
                            //user.Email = model.Email;
                            //user.NormalizedEmail = model.Email.ToUpper();
                            //user.UserName = model.Email;
                            //user.NormalizedUserName = model.Email.ToUpper();

                            var respose = await _userHelper.UpdateUserAsync(user);

                            if (respose.Succeeded)
                            {
                                client.FlownSegments = model.FlownSegments;
                                client.MembershipDate = model.MembershipDate;
                                client.ProgramTierId = model.ProgramTierId;
                                // TODO Fix program tier not changing
                                // model passes Id

                                await _clientRepository.UpdateAsync(client);

                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, respose.Errors.FirstOrDefault().Description);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "User and Client do not match.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Client not found.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User account not found.");
                }
            }

            model.ProgramTiers = _combosHelper.GetProgramTiers();
            return View(model);
        }

        //// GET: Clients/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var client = await _context.Clients
        //        .Include(c => c.ProgramTier)
        //        .Include(c => c.User)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (client == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(client);
        //}

        //// POST: Clients/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var client = await _context.Clients.FindAsync(id);
        //    _context.Clients.Remove(client);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

    }
}
