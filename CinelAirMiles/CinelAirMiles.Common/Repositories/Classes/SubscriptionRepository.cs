namespace CinelAirMiles.Common.Repositories.Classes
{
    using System.Threading.Tasks;
    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;
    using Microsoft.EntityFrameworkCore;

    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        readonly ApplicationDbContext _context;

        public SubscriptionRepository(
            ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Subscriptions.CountAsync();
        }
    }
}
