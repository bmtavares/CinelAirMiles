namespace CinelAirMiles.Web.Backoffice.Helpers.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CinelAirMiles.Common.Entities;
    using CinelAirMiles.Common.Models;
    using CinelAirMiles.Web.Backoffice.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IUserHelper
    {
        Task<List<User>> GetUsersListAsync();

        Task<List<UserViewModel>> GetUsersWithRolesListAsync(List<UserViewModel> models);

        Task<UserViewModel> GetUserWithRoleAsync(UserViewModel model);

        Task ChangeUserRole(UserViewModel model);

        Task<List<User>> GetEmployeesListAsync();

        Task<int> GetEmployeesCountAsync();

        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);

        Task<SignInResult> ValidatePasswordAsync(User user, string password);

        Task CheckRoleAsync(string roleName);

        Task<bool> IsUserInRoleAsync(User user, string roleName);

        Task AddUserToRoleAsync(User user, string roleName);

        Task<string> GenerateEmailConfirmationTokenAsync(User user);

        Task<IdentityResult> ConfirmEmailAsync(User user, string token);

        Task<User> GetUserByIdAsync(string userId);

        Task<string> GeneratePasswordResetTokenAsync(User user);

        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);

        IEnumerable<SelectListItem> GetComboRoles();

        Task<EditUserViewModel> GetEditUserWithRoleAsync(EditUserViewModel model);
    }
}
