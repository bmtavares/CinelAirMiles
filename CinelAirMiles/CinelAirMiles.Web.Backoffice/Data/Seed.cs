namespace CinelAirMiles.Web.Backoffice.Data
{
    using System;
    using System.Threading.Tasks;

    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;
    using CinelAirMiles.Web.Backoffice.Helpers.Interfaces;

    using Microsoft.EntityFrameworkCore;

    public class Seed
    {
        readonly ApplicationDbContext _context;
        readonly IUserHelper _userHelper;

        public Seed(
            ApplicationDbContext context,
            IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;

        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("SuperUser");
            await _userHelper.CheckRoleAsync("User");

            await CreateDefaultUsers();

            if (!await _context.MilesTypes.AnyAsync())
            {
                await CreateMileType("Status");
                await CreateMileType("Bonus");

                await _context.SaveChangesAsync();
            }

            if (!await _context.ProgramTiers.AnyAsync())
            {
                await CreateProgramTier("Basic");
                await CreateProgramTier("Silver");
                await CreateProgramTier("Gold");

                await _context.SaveChangesAsync();
            }
        }

        async Task CreateMileType(string description)
        {
            await _context.MilesTypes.AddAsync(new MilesType
            {
                Description = description
            });
        }

        async Task CreateProgramTier(string description)
        {
            await _context.ProgramTiers.AddAsync(new ProgramTier
            {
                Description = description
            });
        }

        async Task CreateDefaultUsers()
        {
            await CreateDefaultAdmin();
            await CreateDefaultSuperUser();
            await CreateDefaultUser();
        }

        //TODO: Change to a valid e-mail address before publishing
        async Task CreateDefaultAdmin()
        {
            var user = await _userHelper.GetUserByEmailAsync("admin@mail.com");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "Default",
                    LastName = "Admin",
                    Email = "admin@mail.com",
                    UserName = "admin@mail.com",
                    MainRole = "Employee",
                    RequirePasswordChange = false,
                    EmailConfirmed = true
                };

                var result = await _userHelper.AddUserAsync(user, "P@ssw0rd!");

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"An error occurred trying to create the default Admin in the seeder");
                }

                var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");

                if (!isInRole)
                {
                    await _userHelper.AddUserToRoleAsync(user, "Admin");
                }
            }
        }

        //TODO: Remove this method before publishing
        async Task CreateDefaultSuperUser()
        {
            var user = await _userHelper.GetUserByEmailAsync("superuser@mail.com");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "Default",
                    LastName = "SuperUser",
                    Email = "superuser@mail.com",
                    UserName = "superuser@mail.com",
                    MainRole = "Employee",
                    RequirePasswordChange = false,
                    EmailConfirmed = true
                };

                var result = await _userHelper.AddUserAsync(user, "P@ssw0rd!");

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"An error occurred trying to create the default SuperUser in the seeder");
                }

                var isInRole = await _userHelper.IsUserInRoleAsync(user, "SuperUser");

                if (!isInRole)
                {
                    await _userHelper.AddUserToRoleAsync(user, "SuperUser");
                }
            }
        }

        //TODO: Remove this method before publishing
        async Task CreateDefaultUser()
        {
            var user = await _userHelper.GetUserByEmailAsync("user@mail.com");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "Default",
                    LastName = "User",
                    Email = "user@mail.com",
                    UserName = "user@mail.com",
                    MainRole = "Employee",
                    RequirePasswordChange = false,
                    EmailConfirmed = true
                };

                var result = await _userHelper.AddUserAsync(user, "P@ssw0rd!");

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"An error occurred trying to create the default User in the seeder");
                }

                var isInRole = await _userHelper.IsUserInRoleAsync(user, "User");

                if (!isInRole)
                {
                    await _userHelper.AddUserToRoleAsync(user, "User");
                }
            }
        }
    }
}
