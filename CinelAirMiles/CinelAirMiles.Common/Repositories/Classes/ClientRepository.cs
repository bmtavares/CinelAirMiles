namespace CinelAirMiles.Common.Repositories.Classes
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;
    using Microsoft.EntityFrameworkCore;

    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        readonly ApplicationDbContext _context;
        readonly INotificationRepository _notificationRepository;
        readonly Random _random;

        public ClientRepository(
            ApplicationDbContext context,
            INotificationRepository notificationRepository) : base(context)
        {
            _context = context;
            _notificationRepository = notificationRepository;
            _random = new Random();
        }

        public List<Client> GetClientsWithUsers()
        {
            return _context.Clients
                    .Include(c => c.User)
                    .ToList();
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

        public async Task CreateClientWithUserAsync(User user)
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
                ProgramTier = programTier
            };

            await CreateAsync(client);
        }

        public async Task<Client> GetClientByNumberAsync(string number)
            => await _context.Clients.Where(c => c.MilesProgramNumber == number).FirstOrDefaultAsync();

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

        //TODO: Not allow various users to request came change if it's already been requested
        //TODO: Not allow various superusers to confirm or deny a request if another superuser has already confirmed or denied it
        public async Task RequestClientTierChangeAsync(Client client, User user)
        {
            var notification = new Notification
            {
                Text = $"User {user.UserName} has requested a tier change for client number {client.MilesProgramNumber} to tier {client.ProgramTier.Description}"
            };

            await _notificationRepository.CreateNotificationWithUserAndTypeAsync(notification, user.Id, "Alert");
        }

        public async Task EditClientAsync(Client clientChange, User user)
        {
            var client = await _context.Clients
                .Include(c => c.ProgramTier)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == clientChange.Id);

            if(client.ProgramTier.Id == clientChange.ProgramTierId)
            {
                await UpdateAsync(clientChange);
            }
            else
            {
                await RequestClientTierChangeAsync(clientChange, user);

                clientChange.ProgramTier = client.ProgramTier;
                await UpdateAsync(clientChange);
            }
        }
    }
}
