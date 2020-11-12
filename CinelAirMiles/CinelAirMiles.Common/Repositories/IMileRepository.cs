namespace CinelAirMiles.Common.Repositories
{
    using CinelAirMiles.Common.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMileRepository : IGenericRepository<Mile>
    {
        Task<Mile> GetMileWithClientAndTypeAsync(int? id);

        Task<IEnumerable<Mile>> GetMilesAssociatedWithClientAsync(int clientId);
    }
}
