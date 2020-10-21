namespace CinelAirMiles.Common.Repositories
{
    using CinelAirMiles.Common.Entities;
    using System.Threading.Tasks;

    public interface IMileRepository : IGenericRepository<Mile>
    {
        Task<Mile> GetMileWithClientAndType(int? id);
    }
}
