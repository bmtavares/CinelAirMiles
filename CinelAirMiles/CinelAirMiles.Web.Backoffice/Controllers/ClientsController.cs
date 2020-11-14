namespace CinelAirMiles.Web.Backoffice.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;
    using CinelAirMiles.Common.Models;
    using CinelAirMiles.Common.Repositories;
    using CinelAirMiles.Web.Backoffice.Helpers.Interfaces;
    using CinelAirMiles.Web.Backoffice.Models;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;

    [Authorize(Roles = "Admin, User")]
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IClientRepository _clientRepository;
        private readonly ICombosHelper _combosHelper;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IMileRepository _mileRepository;
        private readonly IMilesTransactionRepository _milesTransactionRepository;

        public ClientsController(
            ApplicationDbContext context,
            IClientRepository clientRepository,
            ICombosHelper combosHelper,
            IUserHelper userHelper,
            IConverterHelper converterHelper,
            IMileRepository mileRepository,
            IMilesTransactionRepository milesTransactionRepository)
        {
            _context = context;
            _clientRepository = clientRepository;
            _combosHelper = combosHelper;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _mileRepository = mileRepository;
            _milesTransactionRepository = milesTransactionRepository;
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
            var model = new RegisterNewClientViewModel()
            {
                //ProgramTiers = _combosHelper.GetProgramTiers()
            };

            return View(model);
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterNewClientViewModel model)
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

                    await _clientRepository.CreateClientWithUserAsync(user, model.BirthDate.Value);

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
                                // model passes Id

                                var currentUser = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                                var message = await _clientRepository.EditClientAsync(client, currentUser);

                                ViewData["Message"] = message;


                                model.ProgramTiers = _combosHelper.GetProgramTiers();

                                return View(model);
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

        public async Task<ActionResult> Activate(int? clientId)
        {
            if (clientId == null)
            {
                return NotFound();
            }

            var client = await _clientRepository.GetClientWithDetailsAsync(clientId.Value);

            if (client != null)
            {
                if (!client.Active && !client.IsDeceased)
                {
                    return View(client);
                }
            }

            return NotFound();
        }

        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateConfirm(int id)
        {
            var client = await _clientRepository.GetByIdAsync(id);


            if (client != null)
            {
                if (!client.IsDeceased)
                {
                    client.Active = true;
                    await _clientRepository.UpdateAsync(client);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Deactivate(int? clientId)
        {
            if (clientId == null)
            {
                return NotFound();
            }

            var client = await _clientRepository.GetClientWithDetailsAsync(clientId.Value);

            if (client != null)
            {
                if(client.Active)
                {
                    return View(client);
                }
            }

            return NotFound();
        }

        [HttpPost, ActionName("Deactivate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeactivateConfirm(int id)
        {
            var client = await _clientRepository.GetByIdAsync(id);

            if(client != null)
            {
                if(client.IsInReferrerProgram)
                {
                    //TODO: Remove from referral
                }

                client.Active = false;
                await _clientRepository.UpdateAsync(client);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Inherit(int? clientId)
        {
            if (clientId == null)
            {
                return NotFound();
            }

            var client = await _clientRepository.GetByIdAsync(clientId.Value);
            if (client != null)
            {
                if(!client.Active && !client.IsDeceased)
                {
                    var model = new ClientInheritViewModel
                    {
                        ClientId = client.Id
                    };

                    return View(model);
                }
            }
            
            return NotFound();
            
        }

        // POST: Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Inherit(ClientInheritViewModel model)
        {
            if (ModelState.IsValid)
            {
                var client = await _clientRepository.GetByIdAsync(model.ClientId);
                if (client != null)
                {
                    //if (model.HeirIds.Count() > 0)
                    //{
                        if(!client.Active && !client.IsDeceased)
                        {
                            //foreach (var heirProgramNum in model.HeirIds)
                            //{
                                var heir = await _clientRepository.GetClientByNumberAsync(model.HeirProgramNum);
                                
                                if(heir != null)
                                {
                                    var bonusBalance = await _mileRepository.GetCurrentMilesBalanceByClientIdAsync(client.Id, "Bonus");
                                    var statusBalance = await _mileRepository.GetCurrentMilesBalanceByClientIdAsync(client.Id, "Status");
                                    
                                    if(bonusBalance != 0 || statusBalance != 0)
                                        await _milesTransactionRepository.InheritMilesAsync(heir, bonusBalance, statusBalance);


                                    client = await _clientRepository.GetByIdAsync(model.ClientId);
                                    client.IsDeceased = true;
                                    await _clientRepository.UpdateAsync(client);

                                    return RedirectToAction(nameof(Index));
                                }
                                else
                                {
                                //model.HeirIds.Remove(heirProgramNum);
                                ModelState.AddModelError(string.Empty, "Entered heir is not a valid user.");
                                }
                        //}
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Current client has either been processed for inheritance or is active. Please attempt to start this process from the clients list if you believe this to be an error.");
                        }
                    //}
                    //else
                    //{
                    //    ModelState.AddModelError(string.Empty, "Received heirs list was empty.");
                    //}
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User account not found.");
                }
            }

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
