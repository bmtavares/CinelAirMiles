namespace CinelAirMiles.Common.Repositories.Classes
{
    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;

    public class MileRepository : GenericRepository<Mile>, IMileRepository
    {

        public MileRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
