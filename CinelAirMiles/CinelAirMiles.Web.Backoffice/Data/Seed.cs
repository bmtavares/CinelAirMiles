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

            await CheckPartnersAsync();
            await CheckBenefitsAsync();

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("SuperUser");
            await _userHelper.CheckRoleAsync("User");

            await CreateDefaultUsersAsync();


            if(!await _context.MilesTransactionTypes.AnyAsync())
            {
                await CreateMilesTransactionTypeAsync("Purchase");
                await CreateMilesTransactionTypeAsync("Extension");
                await CreateMilesTransactionTypeAsync("Transfer");
                await CreateMilesTransactionTypeAsync("Conversion");

                await _context.SaveChangesAsync();
            }


            if (!await _context.MilesTypes.AnyAsync())
            {
                await CreateMileTypeAsync("Status");
                await CreateMileTypeAsync("Bonus");

                await _context.SaveChangesAsync();
            }


            if (!await _context.ProgramTiers.AnyAsync())
            {
                await CreateProgramTierAsync("Basic");
                await CreateProgramTierAsync("Silver");
                await CreateProgramTierAsync("Gold");

                await _context.SaveChangesAsync();
            }


            if (!await _context.NotificationsTypes.AnyAsync())
            {
                await CreateNotificationTypeAsync("TierChange");
                await CreateNotificationTypeAsync("Complaint");
                await CreateNotificationTypeAsync("SeatAvailability");
                await CreateNotificationTypeAsync("PartnerReference");
                await CreateNotificationTypeAsync("AdInsertion");

                await _context.SaveChangesAsync();
            }
        }


        async Task CreateMilesTransactionTypeAsync(string description)
        {
            await _context.MilesTransactionTypes.AddAsync(new MilesTransactionType
            {
                Description = description
            });
        }


        async Task CreateNotificationTypeAsync(string type)
        {
            await _context.NotificationsTypes.AddAsync(new NotificationType
            {
                Type = type
            });
        }

        async Task CreateMileTypeAsync(string description)
        {
            await _context.MilesTypes.AddAsync(new MilesType
            {
                Description = description
            });
        }

        async Task CreateProgramTierAsync(string description)
        {
            await _context.ProgramTiers.AddAsync(new ProgramTier
            {
                Description = description
            });
        }

        async Task CreateDefaultUsersAsync()
        {
            await CreateDefaultAdminAsync();
            await CreateDefaultSuperUserAsync();
            await CreateDefaultUserAsync();
        }

        //TODO: Change to a valid e-mail address before publishing
        async Task CreateDefaultAdminAsync()
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
        async Task CreateDefaultSuperUserAsync()
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
        async Task CreateDefaultUserAsync()
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

        private async Task CheckPartnersAsync()
        {
            if (! await _context.Partners.AnyAsync())
            {
                await _context.Partners.AddAsync(new Partner { Name = "SweetDrop", Description= "Acumule e utilize milhas com a SweetDrop, uma grande superfície de retalho.", });
                await _context.Partners.AddAsync(new Partner { Name = "Hoteis Pestanas", Description = "Acumule uma percentragem em estadias em hoteis selecionados." });
                await _context.Partners.AddAsync(new Partner { Name = "Air Arctic", Description = "Acumule e utilize milhas ao voar com a  nossa empresa parceira." });
                await _context.Partners.AddAsync(new Partner { Name = "Banco MeioNovo", Description = "Acumule todos os meses com o Banco MeioNovo." });
                await _context.Partners.AddAsync(new Partner { Name = "Abis", Description = "Alugue um carro com a Abis e acumule milhas na sua conta CinelAirMiles." });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckBenefitsAsync()
        {
            if (! await _context.Benefits.AnyAsync())
            {
                var partner = await _context.Partners.FirstOrDefaultAsync();
                await AddBenefitAsync("Desconto em compras mensal", 15 , partner);
                await AddBenefitAsync("Desconto em cada 100€ em compras", 5, partner);
                await _context.SaveChangesAsync();
            }
        }

        private async Task AddBenefitAsync(string description, float reward, Partner partner)
        {
            await _context.Benefits.AddAsync(new Benefit
            {
                Description = description,
                Reward = reward,
                Partner = partner
            });
        }
    }
}
