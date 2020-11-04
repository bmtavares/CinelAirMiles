namespace CinelAirMiles.Common.Repositories.Classes
{
    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;
    using Microsoft.EntityFrameworkCore;
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
    }
}
