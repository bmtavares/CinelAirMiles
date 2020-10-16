using CinelAirMiles.Common.Entities;
using CinelAirMiles.Web.Backoffice.Data.Entities;
using CinelAirMiles.Web.Backoffice.Helpers.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinelAirMiles.Web.Backoffice.Data
{
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


            var user = await _userHelper.GetUserByEmailAsync("noreply.projetoscinel@gmail.com");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "Default",
                    LastName = "Admin",
                    Email = "noreply.projetoscinel@gmail.com",
                    UserName = "noreply.projetoscinel@gmail.com",
                    EmailConfirmed = true
                };

                var result = await _userHelper.AddUserAsync(user, "ABab12!?");

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"An error occurred trying to create the default Admin Ricardo Lourenço in the seeder");
                }

                var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");

                if (!isInRole)
                {
                    await _userHelper.AddUserToRoleAsync(user, "Admin");
                }
            }

            if (!await _context.MilesTypes.AnyAsync())
            {
                CreateMileType("Status");
                CreateMileType("Bonus");

                await _context.SaveChangesAsync();
            }

            if (!await _context.ProgramTiers.AnyAsync())
            {
                CreateProgramTier("Basic");
                CreateProgramTier("Silver");
                CreateProgramTier("Gold");

                await _context.SaveChangesAsync();
            }
        }

        void CreateMileType(string description)
        {
            _context.MilesTypes.Add(new MilesType
            {
                Description = description
            });
        }

        void CreateProgramTier(string description)
        {
            _context.ProgramTiers.Add(new ProgramTier
            {
                Description = description
            });
        }
    }
}
