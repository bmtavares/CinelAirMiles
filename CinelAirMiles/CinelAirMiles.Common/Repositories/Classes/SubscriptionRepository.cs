using CinelAirMiles.Common.Data;
using CinelAirMiles.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinelAirMiles.Common.Repositories.Classes
{
    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        readonly ApplicationDbContext _context;

        public SubscriptionRepository(
            ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
