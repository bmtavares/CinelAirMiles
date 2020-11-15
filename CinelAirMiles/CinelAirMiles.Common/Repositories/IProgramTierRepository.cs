namespace CinelAirMiles.Common.Repositories
{
    using CinelAirMiles.Common.Entities;
    using System.Threading.Tasks;

    public interface IProgramTierRepository : IGenericRepository<ProgramTier>
    {
        Task<ProgramTier> GetByDescriptionAsync(string description);
    }
}
