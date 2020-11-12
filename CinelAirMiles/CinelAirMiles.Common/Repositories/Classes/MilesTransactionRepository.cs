namespace CinelAirMiles.Common.Repositories.Classes
{
    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;
    using CinelAirMiles.Common.Repositories;
    using System.Threading.Tasks;

    public class MilesTransactionRepository : GenericRepository<MilesTransaction>, IMilesTransactionRepository
    {
        readonly ApplicationDbContext _context;

        public MilesTransactionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
