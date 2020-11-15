namespace CinelAirMiles.Common.Repositories
{
    using System.Threading.Tasks;

    using CinelAirMiles.Common.Entities;

    public interface IProgramTierRepository : IGenericRepository<ProgramTier>
    {
        /// <summary>
        /// Returns tier multiplier for miles calculation
        /// </summary>
        /// <param name="id">ProgramTier Id</param>
        /// <returns>Double multiplier</returns>
        Task<double> GetMultiplierByIdAsync(int id);
    }
}
