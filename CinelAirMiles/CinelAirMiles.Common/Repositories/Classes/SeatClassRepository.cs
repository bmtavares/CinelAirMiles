namespace CinelAirMiles.Common.Repositories.Classes
{
    using System.Linq;
    using System.Threading.Tasks;

    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;

    using Microsoft.EntityFrameworkCore;

    public class SeatClassRepository : GenericRepository<SeatClass>, ISeatClassRepository
    {
        private readonly ApplicationDbContext _context;

        public SeatClassRepository(
            ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<double> GetRegularMultiplierByIdAsync(int id)
        {
            return await _context.SeatClasses
                .Where(sc => sc.Id == id)
                .Select(sc => sc.RegularMultiplier)
                .FirstOrDefaultAsync();
        }

        public async Task<double> GetInternationalMultiplierByIdAsync(int id)
        {
            return await _context.SeatClasses
                .Where(sc => sc.Id == id)
                .Select(sc => sc.InternationalMultiplier)
                .FirstOrDefaultAsync();
        }
    }
}
