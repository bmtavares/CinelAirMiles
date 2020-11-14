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
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Mile> GetMileWithClientAndTypeAsync(int? id);

        /// <summary>
        /// Returns all Miles associated with a Client with matching ID from the context
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<IEnumerable<Mile>> GetMilesAssociatedWithClientAsync(int clientId);
    }
}
