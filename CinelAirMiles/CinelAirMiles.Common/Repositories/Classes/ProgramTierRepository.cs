namespace CinelAirMiles.Common.Repositories.Classes
{
    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;

    public class ProgramTierRepository : GenericRepository<ProgramTier>, IProgramTierRepository
    {
        public ProgramTierRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
