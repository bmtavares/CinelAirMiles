namespace CinelAirMiles.Common.Repositories.Classes
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;
    using Microsoft.EntityFrameworkCore;

    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        readonly ApplicationDbContext _context;
        readonly Random _random;

        public ClientRepository(
            ApplicationDbContext context) : base(context)
        {
            _context = context;
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

        public async Task<Client> GetClientByNumber(string number)
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
    }
}
