namespace CinelAirMiles.Web.Backoffice.Controllers
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    using CinelAirMiles.Common.Entities;
    using CinelAirMiles.Web.Backoffice.Helpers.Interfaces;
    using CinelAirMiles.Common.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.AspNetCore.Authorization;
    using CinelAirMiles.Web.Backoffice.Models;

    //TODO: User is given a predefined password given by the admin at first, and must change it on first login
    public class AccountController : Controller
    {
        readonly IUserHelper _userHelper;
        readonly IConfiguration _configuration;
        readonly IMailHelper _mailHelper;
        readonly IConverterHelper _converterHelper;

        public AccountController(
            IUserHelper userHelper,
            IConfiguration configuration,
            IMailHelper mailHelper,
            IConverterHelper converterHelper)
        {
            _userHelper = userHelper;
            _configuration = configuration;
            _mailHelper = mailHelper;
            _converterHelper = converterHelper;
        }

        [Route("Login")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);

                if(user != null)
                {
                    if (await _userHelper.IsUserInRoleAsync(user, "Employee"))
                    {
                        var result = await _userHelper.LoginAsync(model);

                        if (result.Succeeded)
                        {
                            if (user.RequirePasswordChange)
                            {
                                await _userHelper.LogoutAsync();

                                var gennedToken = await _userHelper.GeneratePasswordResetTokenAsync(user);

                                return RedirectToAction("ForcedChange", new { token = gennedToken, username = user.UserName });
                            }

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

        public IActionResult ForcedChange(string token, string username)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForcedChange(ResetPasswordViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.UserName);
            if (user != null)
            {
                var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    user.RequirePasswordChange = false;

                    await _userHelper.UpdateUserAsync(user);

                    this.ViewBag.Message = "Password reset successful. Please login.";
                    return this.View();
                }

                this.ViewBag.Message = "Error while resetting the password.";
                return View(model);
            }

            this.ViewBag.Message = "User not found.";
            return View(model);
        }

        //[Authorize(Roles = "Admin")]
        //public IActionResult Register()
        //{
        //    var model = new RegisterNewUserViewModel
        //    {
        //        //Countries = _countryRepository.GetComboCountries(),
        //        //Cities = _countryRepository.GetComboCities(0)
        //    };

        //    View(model);

        //    return View();
        //}

        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        //public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userHelper.GetUserByEmailAsync(model.Username);

        //        if (user == null)
        //        {
        //            //var city = await _countryRepository.GetCityAsync(model.CityId);

        //            user = new User
        //            {
        //                FirstName = model.FirstName,
        //                LastName = model.LastName,
        //                Email = model.Username,
        //                UserName = model.Username,
        //                PhoneNumber = model.PhoneNumber
        //            };

        //            var result = await _userHelper.AddUserAsync(user, model.Password);

        //            if (result != IdentityResult.Success)
        //            {
        //                ModelState.AddModelError(string.Empty, "The user couldn't be created");
        //                return View(model);
        //            }

        //            //var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
        //            //var tokenLink = Url.Action("ConfirmEmail", "Account", new
        //            //{
        //            //    userid = user.Id,
        //            //    token = myToken
        //            //}, protocol: HttpContext.Request.Scheme);

        //            //_mailHelper.SendMail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
        //            //    $"To allow the user, " +
        //            //    $"plase click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");
        //            //ViewBag.Message = "The instructions to allow your user has been sent to email.";

        //            return View(model);
        //        }

        //        ModelState.AddModelError(string.Empty, "This username already exists");
        //    }

        //    return View(model);
        //}

        //public async Task<IActionResult> ConfirmEmail(string userId, string token)
        //{
        //    if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
        //    {
        //        return NotFound();
        //    }

        //    var user = await _userHelper.GetUserByIdAsync(userId);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    var result = await _userHelper.ConfirmEmailAsync(user, token);
        //    if (!result.Succeeded)
        //    {
        //        return NotFound();
        //    }

        //    return View();
        //}

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

        //public IActionResult RecoverPassword()
        //{
        //    return this.View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        //{
        //    if (this.ModelState.IsValid)
        //    {
        //        var user = await _userHelper.GetUserByEmailAsync(model.Email);
        //        if (user == null)
        //        {
        //            ModelState.AddModelError(string.Empty, "The email doesn't correspont to a registered user.");
        //            return this.View(model);
        //        }

        //        var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);

        //        var link = this.Url.Action(
        //            "ResetPassword",
        //            "Account",
        //            new { token = myToken }, protocol: HttpContext.Request.Scheme);

        //        _mailHelper.SendMail(model.Email, "Shop Password Reset", $"<h1>Shop Password Reset</h1>" +
        //        $"To reset the password click in this link:</br></br>" +
        //        $"<a href = \"{link}\">Reset Password</a>");
        //        this.ViewBag.Message = "The instructions to recover your password has been sent to email.";
        //        return this.View();

        //    }

        //    return this.View(model);
        //}

        //public IActionResult ResetPassword(string token)
        //{
        //    return View();
        //}


        //[HttpPost]
        //public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    var user = await _userHelper.GetUserByEmailAsync(model.UserName);
        //    if (user != null)
        //    {
        //      var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
        //        if (result.Succeeded)
        //        {
        //            this.ViewBag.Message = "Password reset successful.";
        //            return this.View();
        //        }

        //        this.ViewBag.Message = "Error while resetting the password.";
        //        return View(model);
        //    }

        //    this.ViewBag.Message = "User not found.";
        //    return View(model);
        //}

        //public async Task<IActionResult> ChangeUser()
        //{
        //    var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
        //    var model = new ChangeUserViewModel();

        //    if (user != null)
        //    {
        //        model.FirstName = user.FirstName;
        //        model.LastName = user.LastName;
        //        model.PhoneNumber = user.PhoneNumber;

        //        //var city = await _countryRepository.GetCityAsync(user.CityId);
        //        //if (city != null)
        //        //{
        //        //    var country = await _countryRepository.GetCountryAsync(city);
        //        //    if (country != null)
        //        //    {
        //        //        model.CountryId = country.Id;
        //        //        model.Cities = _countryRepository.GetComboCities(country.Id);
        //        //        model.Countries = _countryRepository.GetComboCountries();
        //        //        model.CityId = user.CityId;
        //        //    }
        //        //}
        //    }

        //    //model.Cities = _countryRepository.GetComboCities(model.CountryId);
        //    //model.Countries = _countryRepository.GetComboCountries();
        //    return this.View(model);
        //}



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



        //public IActionResult ChangePassword()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
        //        if (user != null)
        //        {
        //            var result = await _userHelper.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        //            if (result.Succeeded)
        //            {
        //                return RedirectToAction("ChangeUser");
        //            }
        //            else
        //            {
        //                ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
        //            }
        //        }
        //        else
        //        {
        //            ModelState.AddModelError(string.Empty, "User not found.");
        //        }
        //    }

        //    return View(model);
        //}

        [Route("NotAuthorized")]
        public IActionResult NotAuthorized()
        {
            return View();
        }

        //public async Task<JsonResult> GetCitiesAsync(int CountryId)
        //{
        //    //var country = await _countryRepository.GetCountryWithCitiesAsync(CountryId);

        //    //return Json(country.Cities.OrderBy(c => c.Name));
        //}

        #region Admin CRUD
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await _userHelper.GetEmployeesListAsync();

            var models = _converterHelper.UsersToUserViewModels(users);

            models = await _userHelper.GetUsersWithRolesListAsync(models);

            return View(models);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(id);


            if (user == null)
            {
                return NotFound();
            }

            var model = _converterHelper.UserToUserViewModel(user);

            model = await _userHelper.GetUserWithRoleAsync(model);

            return View(model);
        }


       [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(RegisterNewUserViewModel model)
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
                        RequirePasswordChange = true
                    };

                    var result = await _userHelper.AddUserAsync(user, model.Password);

                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user could not be created.");
                        return View(model);
                    }

                    var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);

                    await _userHelper.ConfirmEmailAsync(user, token);

                    await _userHelper.AddUserToRoleAsync(user, "Employee");

                    return RedirectToAction("Index", "Account");

                }

                ModelState.AddModelError(string.Empty, "The username is already taken.");

            }

            return View(model);
        }

        [Authorize(Roles = "Admin")] //TODO : Eventually change password/email (also implement in post)
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(id);
            //var model = new EditUserViewModel();

            if (user == null)
            {
                return NotFound();
            }

            //model.FirstName = user.FirstName;
            //model.LastName = user.LastName;
            //model.PhoneNumber = user.PhoneNumber;
            //model.Id = id;

            var model = _converterHelper.UserToEditUserViewModel(user);

            model.Roles = _userHelper.GetComboRoles();

            model = await _userHelper.GetEditUserWithRoleAsync(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByIdAsync(model.Id);
                if (user != null)
                {
                    //TODO: VERY IMPORTANT: logout respective user if e-mail is changed by the admin
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Email = model.Email;
                    user.NormalizedEmail = model.Email.ToUpper();
                    user.UserName = model.Email;
                    user.NormalizedUserName = model.Email.ToUpper();




                    var respose = await _userHelper.UpdateUserAsync(user);

                    await _userHelper.AddUserToRoleAsync(user, model.RoleName);

                    if (respose.Succeeded)
                    {
                        return RedirectToAction("Index", "Account");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, respose.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                }
            }

            return View(model);
        }

        // GET: User/Delete/5
        //[Authorize(Roles = "Admin")]
        //[Route("User/Delete")]
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (string.IsNullOrEmpty(id))
        //    {
        //        return NotFound();
        //    }

        //    var user = await _userHelper.GetUserByIdAsync(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}

        // POST: User/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        //[Route("User/Delete")]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    var user = await _userHelper.GetUserByIdAsync(id);

        //    await _userHelper.DeleteUserAsync(user);
        //    return RedirectToAction(nameof(Index));
        //}

        #endregion
    }
}