using CinelAirMiles.Common.Data;
using CinelAirMiles.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinelAirMiles.Common.Repositories.Classes
{
    public class PartnerRepository : GenericRepository<Partner>, IPartnerRepository
    {
        private readonly ApplicationDbContext _context;

        public PartnerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
