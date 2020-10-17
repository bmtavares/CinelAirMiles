namespace CinelAirMiles.Common.Repositories.Classes
{
    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;
    using CinelAirMiles.Common.Repositories;

    public class MilesTransactionRepository : GenericRepository<MilesTransaction>, IMilesTransactionRepository
    {

        public MilesTransactionRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
