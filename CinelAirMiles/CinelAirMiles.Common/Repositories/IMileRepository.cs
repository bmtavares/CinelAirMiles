namespace CinelAirMiles.Common.Repositories
{
    using CinelAirMiles.Common.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMileRepository : IGenericRepository<Mile>
    {
        /// <summary>
        /// Returns Mile with the respective ID, including its associated Client and Type from the context
        /// </summary>
        /// <param name="id">Id for Mile</param>
        /// <returns>Mile with Client and MileType</returns>
        Task<Mile> GetMileWithClientAndTypeAsync(int? id);

        /// <summary>
        /// Returns all Miles associated with a Client with matching ID from the context
        /// </summary>
        /// <param name="clientId">Id for Client</param>
        /// <returns>Miles</returns>
        Task<IEnumerable<Mile>> GetMilesAssociatedWithClientAsync(int clientId);

        /// <summary>
        /// Returns the selected MileType miles balance for a Client with matching ID from the context
        /// </summary>
        /// <param name="clientId">Id for Client</param>
        /// <param name="mileType">slug for MileType</param>
        /// <returns>Current balance</returns>
        Task<int> GetCurrentMilesBalanceByClientIdAsync(int clientId, string mileType);
    }
}
