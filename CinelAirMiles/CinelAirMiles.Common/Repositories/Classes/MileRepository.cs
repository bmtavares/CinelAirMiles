namespace CinelAirMiles.Common.Repositories.Classes
{
    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MileRepository : GenericRepository<Mile>, IMileRepository
    {
        readonly ApplicationDbContext _context;

        public MileRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Mile> GetMileWithClientAndTypeAsync(int? id)
        {
            return await _context.Miles
                .Include(m => m.Client)
                .Include(m => m.MilesType)
                .Where(m => m.Id == id.Value)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Mile>> GetMilesAssociatedWithClientAsync(int clientId)
        {
            var client = await _context.Clients
                .Include(c => c.Miles.Where(m => m.ExpiryDate >= DateTime.UtcNow))
                .ThenInclude(m => m.MilesType)
                .FirstOrDefaultAsync(c => c.Id == clientId);

            return client.Miles.OrderByDescending(m => m.ExpiryDate);
        }

        public async Task<int> GetCurrentMilesBalanceByClientIdAsync(int clientId, string mileType)
        {
            var mileTypeId = await _context.MilesTypes
                .Where(mt => mt.Description == mileType)
                .Select(mt => mt.Id)
                .FirstOrDefaultAsync();

            var balance = await _context.Miles
                                    .Where(mt => mt.MilesTypeId == mileTypeId)
                                    .Where(m => m.ExpiryDate >= DateTime.UtcNow)
                                    .Where(m => m.ClientId == clientId)
                                    .Select(m => m.Balance)
                                    .SumAsync();

            return balance;
        }
    }
}
