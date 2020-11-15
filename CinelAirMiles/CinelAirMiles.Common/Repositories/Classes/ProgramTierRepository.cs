namespace CinelAirMiles.Common.Repositories.Classes
{
    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProgramTierRepository : GenericRepository<ProgramTier>, IProgramTierRepository
    {
        readonly ApplicationDbContext _context;

        public ProgramTierRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ProgramTier> GetByDescriptionAsync(string description) =>
            await _context.ProgramTiers.FirstOrDefaultAsync(pt => pt.Description == description);
    }
}
