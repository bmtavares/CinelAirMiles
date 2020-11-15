namespace CinelAirMiles.Common.Repositories.Classes
{
    using System.Linq;
    using System.Threading.Tasks;

    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;

    using Microsoft.EntityFrameworkCore;

    public class ProgramTierRepository : GenericRepository<ProgramTier>, IProgramTierRepository
    {
        private readonly ApplicationDbContext _context;

        public ProgramTierRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<double> GetMultiplierByIdAsync(int id)
        {
            return await _context.ProgramTiers
                .Where(pt => pt.Id == id)
                .Select(pt => pt.MilesMultiplier)
                .FirstOrDefaultAsync();
        }
    }
}
