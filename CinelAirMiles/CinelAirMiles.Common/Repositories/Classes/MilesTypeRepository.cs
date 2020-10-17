namespace CinelAirMiles.Common.Repositories.Classes
{
    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;
    public class MilesTypeRepository : GenericRepository<MilesType>, IMilesTypeRepository
    {
        public MilesTypeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
