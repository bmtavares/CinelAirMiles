namespace CinelAirMiles.Web.Frontoffice.Controllers
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using CinelAirMiles.Common.Models;
    using CinelAirMiles.Common.Repositories;
    using CinelAirMiles.Common.Entities;
    using CinelAirMiles.Web.Frontoffice.Helpers.Interfaces;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;

    public class AccountController : Controller
    {
        readonly IUserHelper _userHelper;
        readonly IConfiguration _configuration;
        readonly IMailHelper _mailHelper;
        readonly IClientRepository _clientRepository;

        public AccountController(
            IUserHelper userHelper,
            IConfiguration configuration,
            IMailHelper mailHelper,
            IClientRepository clientRepository)
        {
            _userHelper = userHelper;
            _configuration = configuration;
            _mailHelper = mailHelper;
            _clientRepository = clientRepository;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(MembershipLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var client = await _clientRepository.GetClientByNumberAsync(model.ProgramNumber);

                if(client != null)
                {
                    var user = await _userHelper.GetUserByIdAsync(client.UserId);

                    if(user != null)
                    {
                        var loginModel = new LoginViewModel
                        {
                            Username = user.UserName,
                            Password = model.Password,
                            RememberMe = model.RememberMe
                        };

                        var result = await _userHelper.LoginAsync(loginModel);

                        if (result.Succeeded)
                        {
                            if (Request.Query.Keys.Contains("ReturnUrl"))
                            {
                                return Redirect(Request.Query["ReturnUrl"].First());
                            }
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }

            ModelState.AddModelError(string.Empty, "Failed to login");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            var model = new RegisterNewClientViewModel
            {
                //Countries = _countryRepository.GetComboCountries(),
                //Cities = _countryRepository.GetComboCities(0)
            };

            View(model);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewClientViewModel model)
        {
            if (ModelState.IsValid)
            {
                var today = DateTime.Today;

                var age = today.Year - model.BirthDate.Value.Year;

                if (model.BirthDate.Value.Date > today.AddYears(-age)) { age--; }

                if(age >= 2)
                {
                    var user = await _userHelper.GetUserByEmailAsync(model.Username);

                    if (user == null)
                    {
                        //var city = await _countryRepository.GetCityAsync(model.CityId);

                        user = new User
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Username,
                            UserName = model.Username,
                            PhoneNumber = model.PhoneNumber,

                        };

                        var result = await _userHelper.AddUserAsync(user, model.Password);



                        if (result != IdentityResult.Success)
                        {
                            ModelState.AddModelError(string.Empty, "The user couldn't be created");
                            return View(model);
                        }

                        //TODO: Error message
                        await _clientRepository.CreateClientWithUserAsync(user, model.BirthDate.Value);

                        var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                        var tokenLink = Url.Action("ConfirmEmail", "Account", new
                        {
                            userid = user.Id,
                            token = myToken
                        }, protocol: HttpContext.Request.Scheme);

                        var client = await _clientRepository.GetClientByUserAsync(user);

                        //TODO Prettify Email
                        _mailHelper.SendMail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                            "Welcome to CinelAirMiles!<br/><br/>" +
                            $"To confirm that this is your email, <a href = \"{tokenLink}\">click here</a> and we will activate your account.<br/>" +
                            $"After that is done, you may login using your personal, newly issued, Program Number: <b>{client.MilesProgramNumber}</b>");
                        ViewBag.Message = "The instructions to allow your user has been sent to email.";

                        return View(model);
                    }

                    ModelState.AddModelError(string.Empty, "This username already exists");
                }

                ModelState.AddModelError("BirthDate", "The client must be older than 2 years old");
            }

            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user != null)
                {
                    var result = await _userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(15),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return Created(string.Empty, results);
                    }
                }
            }

            return BadRequest();
        }

        public IActionResult RecoverPassword()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "The email doesn't correspont to a registered user.");
                    return this.View(model);
                }

                var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);

                var link = this.Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);

                _mailHelper.SendMail(model.Email, "Shop Password Reset", $"<h1>Shop Password Reset</h1>" + //TODO change shop
                $"To reset the password click in this link:</br></br>" +
                $"<a href = \"{link}\">Reset Password</a>");
                this.ViewBag.Message = "The instructions to recover your password has been sent to email.";
                return this.View();

            }

            return this.View(model);
        }

        public IActionResult ResetPassword(string token)
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.UserName);
            if (user != null)
            {
                var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    this.ViewBag.Message = "Password reset successful.";
                    return this.View();
                }

                this.ViewBag.Message = "Error while resetting the password.";
                return View(model);
            }

            this.ViewBag.Message = "User not found.";
            return View(model);
        }

        public async Task<IActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var model = new ChangeUserViewModel();

            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.PhoneNumber = user.PhoneNumber;

                //var city = await _countryRepository.GetCityAsync(user.CityId);
                //if (city != null)
                //{
                //    var country = await _countryRepository.GetCountryAsync(city);
                //    if (country != null)
                //    {
                //        model.CountryId = country.Id;
                //        model.Cities = _countryRepository.GetComboCities(country.Id);
                //        model.Countries = _countryRepository.GetComboCountries();
                //        model.CityId = user.CityId;
                //    }
                //}
            }

            //model.Cities = _countryRepository.GetComboCities(model.CountryId);
            //model.Countries = _countryRepository.GetComboCountries();
            return this.View(model);
        }



        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    //var city = await _countryRepository.GetCityAsync(model.CityId);

                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.PhoneNumber = model.PhoneNumber;
                    //user.CityId = model.CityId;
                    //user.City = city;

                    var respose = await _userHelper.UpdateUserAsync(user);
                    if (respose.Succeeded)
                    {
                        this.ViewBag.UserMessage = "User updated!";
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, respose.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User no found.");
                }
            }

            return this.View(model);
        }



        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                }
            }

            return View(model);
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }

        //public async Task<JsonResult> GetCitiesAsync(int CountryId)
        //{
        //    //var country = await _countryRepository.GetCountryWithCitiesAsync(CountryId);

        //    //return Json(country.Cities.OrderBy(c => c.Name));
        //}


        #region Client

        public async Task<IActionResult> MyAccount()
        {
            var client = await _clientRepository.GetClientByEmailAsync(this.User.Identity.Name);

            return View(client);
        }

        public async Task<IActionResult> MyStatus()
        {
            return View();
        }

        public async Task<IActionResult> MyBalance()
        {
            return View();
        }

        public async Task<IActionResult> ManageMiles()
        {
            return View();
        }


        #endregion
    }
}