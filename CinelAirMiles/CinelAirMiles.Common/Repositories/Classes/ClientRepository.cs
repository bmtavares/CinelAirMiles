namespace CinelAirMiles.Common.Repositories.Classes
{

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;
    using Microsoft.EntityFrameworkCore;

    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        readonly ApplicationDbContext _context;
        Random _random;

        public ClientRepository(
            ApplicationDbContext context) : base(context)
        {
            _context = context;
            _random = new Random();
        }

        public async Task CreateClientWithUser(User user)
        {
            var programTier =
                await _context.ProgramTiers
                .Where(pt => pt.Description == "Basic")
                .FirstOrDefaultAsync();

            var allProgramNumbers = _context.Clients
                .Select(c => c.MilesProgramNumber);

            var currentNumber = _random.Next(100000000, 999999999).ToString();

            foreach (var number in allProgramNumbers)
            {
                if (currentNumber == number)
                {
                    currentNumber = _random.Next(100000000, 999999999).ToString();
                }
            }

            var client = new Client
            {
                User = user,
                MembershipDate = DateTime.UtcNow,
                Active = true,
                IsInReferrerProgram = false,
                MilesProgramNumber = currentNumber,
                ProgramTier = programTier
            };
        }
    }
}
