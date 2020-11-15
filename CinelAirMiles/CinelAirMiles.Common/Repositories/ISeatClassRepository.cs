namespace CinelAirMiles.Common.Repositories
{
    using System.Threading.Tasks;

    using CinelAirMiles.Common.Entities;

    public interface ISeatClassRepository : IGenericRepository<SeatClass>
    {
        /// <summary>
        /// Returns regular multiplier for miles calculation.
        /// </summary>
        /// <param name="id">SeatClass Id</param>
        /// <returns>Seat multiplier</returns>
        Task<double> GetRegularMultiplierByIdAsync(int id);


        /// <summary>
        /// Returns international multiplier for miles calculation.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Seat multiplier</returns>
        Task<double> GetInternationalMultiplierByIdAsync(int id);
    }
}
