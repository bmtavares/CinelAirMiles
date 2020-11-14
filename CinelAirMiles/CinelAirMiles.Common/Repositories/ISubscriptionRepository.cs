namespace CinelAirMiles.Common.Repositories
{
    using CinelAirMiles.Common.Entities;

    using System.Threading.Tasks;

    public interface ISubscriptionRepository : IGenericRepository<Subscription>
    {
        /// <summary>
        /// Returns subscriptions count.
        /// </summary>
        /// <returns>Subscriptions count</returns>
        Task<int> GetCountAsync();
    }
}
