namespace CinelAirMiles.Common.Repositories.Classes
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        readonly ApplicationDbContext _context;
        readonly UserManager<User> _userManager;
        readonly Random _random;

        public ClientRepository(
            ApplicationDbContext context,
            UserManager<User> userManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
            _random = new Random();
        }

        public List<Client> GetClientsWithUsers()
        {
            return _context.Clients
                    .Include(c => c.User)
                    .Where(c => !c.IsDeceased)
                    .ToList();
        }

        public async Task<int> GetClientsCountAsync()
        {
            return await _context.Clients
                .Where(c => c.Active).CountAsync();
        }

        public async Task<Client> GetClientWithDetailsAsync(int? id)
        {
            return await _context.Clients
                    .Include(c => c.User)
                    .Include(c => c.ProgramTier)
                    .Where(c => c.Id == id.Value)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
        }

        public async Task CreateClientWithUserAsync(User user, DateTime birthDate)
        {
            var programTier =
                await _context.ProgramTiers
                .Where(pt => pt.Description == "Basic")
                .FirstOrDefaultAsync();

            var programNumber = await GenerateProgramNumberAsync();

            var client = new Client
            {
                User = user,
                MembershipDate = DateTime.UtcNow,
                Active = true,
                IsInReferrerProgram = false,
                MilesProgramNumber = programNumber,
                ProgramTier = programTier,
                BirthDate = birthDate
            };

            await CreateAsync(client);
        }

        public async Task<Client> GetClientByNumberAsync(string number)
            => await _context.Clients.Where(c => c.MilesProgramNumber == number).FirstOrDefaultAsync();

        public async Task<Client> GetClientByUserAsync(User user)
            => await _context.Clients.Where(c => c.UserId == user.Id).FirstOrDefaultAsync();

        async Task<string> GenerateProgramNumberAsync()
        {
            var programNumber = _random.Next(100000000, 1000000000).ToString();

            var existingProgramNumbers = _context.Clients
                .Select(c => c.MilesProgramNumber);

            var exists = await existingProgramNumbers.AnyAsync(n => n == programNumber);

            while (exists)
            {
                programNumber = _random.Next(100000000, 1000000000).ToString();

                exists = await existingProgramNumbers.AnyAsync(n => n == programNumber);
            }

            return programNumber;
        }

        async Task RequestClientTierChangeAsync(Client clientWithNewTier, User user)
        {
            var tier = await _context.ProgramTiers.FirstOrDefaultAsync(pt => pt.Id == clientWithNewTier.ProgramTierId);

            var notification = new Notification
            {
                Text = $"User {user.UserName} has requested a tier change for client number {clientWithNewTier.MilesProgramNumber} from {clientWithNewTier.ProgramTier.Description} to {tier.Description}"
            };

            clientWithNewTier.ProgramTierId = clientWithNewTier.ProgramTier.Id;

            await _context.ChangeClientsTierTemp.AddAsync(new ChangeClientTierTemp
            {
                ClientId = clientWithNewTier.Id,
                ProgramTierId = tier.Id
            });

            await _context.SaveChangesAsync();

            var tempTable = await _context.ChangeClientsTierTemp.FirstOrDefaultAsync(cc => cc.ClientId == clientWithNewTier.Id);

            await CreateNotificationWithUserAndTypeAsync(notification, user.Id, tempTable.Id, "TierChange");
        }

        public async Task<string> EditClientAsync(Client clientWithNewTier, User user)
        {
            var clientWithOldTier = await _context.Clients
                .Include(c => c.ProgramTier)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == clientWithNewTier.Id);

            if(clientWithOldTier.ProgramTier.Id == clientWithNewTier.ProgramTierId)
            {
                await UpdateAsync(clientWithNewTier);

                return "Changes saved successfully";
            }
            else
            {
                var pendingClientChange = _context.ChangeClientsTierTemp.Any(cc => cc.ClientId == clientWithNewTier.Id);

                if(pendingClientChange == false)
                {
                    await RequestClientTierChangeAsync(clientWithNewTier, user);

                    clientWithNewTier.ProgramTier = clientWithOldTier.ProgramTier;
                    await UpdateAsync(clientWithNewTier);

                    return "Tier change was requested to a Super user\nOther changes saved successfully";
                }
                else
                {
                    clientWithNewTier.ProgramTier = clientWithOldTier.ProgramTier;
                    await UpdateAsync(clientWithNewTier);
                    return "Tier change request not saved because there is already a pending tier change to this user\nOther changes saved successfully";
                }
            }
        }



        async Task CreateNotificationWithUserAndTypeAsync(Notification notification, string userId, int tempTableId, string notificationType)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                //TODO: Proper error treatment
                return;
            }

            var type = await _context.NotificationsTypes.FirstOrDefaultAsync(nt => nt.Type == notificationType);

            if (type == null)
            {
                //TODO: Proper error treatment
                return;
            }

            notification.NotificationType = type;
            notification.TempTableId = tempTableId;

            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();

            var superUsers = await _userManager.GetUsersInRoleAsync("SuperUser");

            foreach (var superUser in superUsers)
            {
                await _context.NotificationsUsers.AddAsync(new NotificationUser
                {
                    Notification = notification,
                    User = superUser
                });
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Client> GetClientByEmailAsync(string username)
        {
            //return await _context.Clients
            //    .Include(c => c.User)
            //    .Join(_context.Users.Where(u => u.UserName == username),
            //            c => c.UserId, u => u.Id, (c, u) => c).FirstOrDefaultAsync();

            return await _context.Clients
                .Include(c => c.User)
                .Include(c => c.ProgramTier)
                .FirstOrDefaultAsync(c => c.User.UserName == username);
        }
    }
}
