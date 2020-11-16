namespace CinelAirMiles.Common.Repositories
{
    using CinelAirMiles.Common.Entities;
    using System.Threading.Tasks;

    public interface IProgramTierRepository : IGenericRepository<ProgramTier>
    {
        /// <summary>
        /// Returns tier multiplier for miles calculation
        /// </summary>
        /// <param name="id">ProgramTier Id</param>
        /// <returns>Double multiplier</returns>
        Task<double> GetMultiplierByIdAsync(int id);

        /// <summary>
        /// Returns program tier by description
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        Task<ProgramTier> GetByDescriptionAsync(string description);
    }
}
