namespace CinelAirMiles.Web.Backoffice.Helpers.Classes
{
    using System.Threading.Tasks;

    using CinelAirMiles.Common.Entities;
    using CinelAirMiles.Web.Backoffice.Helpers.Interfaces;
    using CinelAirMiles.Common.Models;

    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using CinelAirMiles.Web.Backoffice.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class UserHelper : IUserHelper
    {
        readonly UserManager<User> _userManager;
        readonly SignInManager<User> _signInManager;
        readonly RoleManager<IdentityRole> _roleManager;


        public UserHelper(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<List<User>> GetUsersListAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<List<UserViewModel>> GetUsersWithRolesListAsync(
            List<UserViewModel> models)
        {
            foreach(var model in models)
            {
                var user = await _userManager.FindByIdAsync(model.Id);

                var roles = await _userManager.GetRolesAsync(user);

                model.RoleName = roles.FirstOrDefault();
            }

            return models;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, userRoles);

            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberMe,
                false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(user, token, password);
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<SignInResult> ValidatePasswordAsync(User user, string password)
        {
            return await _signInManager.CheckPasswordSignInAsync(
                user,
                password,
                false);
        }

        public async Task<UserViewModel> GetUserWithRoleAsync(UserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            var roles = await _userManager.GetRolesAsync(user);

            model.RoleName = roles.FirstOrDefault();

            return model;
        }

        //TODO: Output message if successful or error
        public async Task ChangeUserRole(UserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            await AddUserToRoleAsync(user, model.RoleName);
        }

        public IEnumerable<SelectListItem> GetComboRoles()
        {
            var list = _roleManager.Roles.ToList().Select(
                c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Name
                }).ToList();

            return list;
        }

        public async Task<EditUserViewModel> GetEditUserWithRoleAsync(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            var roles = await _userManager.GetRolesAsync(user);

            model.RoleName = roles.FirstOrDefault();

            return model;
        }
    }
}
