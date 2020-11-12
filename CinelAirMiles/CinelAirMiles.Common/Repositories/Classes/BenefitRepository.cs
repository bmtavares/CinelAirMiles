using CinelAirMiles.Common.Data;
using CinelAirMiles.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinelAirMiles.Common.Repositories.Classes
{
    public class BenefitRepository : GenericRepository<Benefit>, IBenefitRepository
    {
        private readonly ApplicationDbContext _context;

        public BenefitRepository(ApplicationDbContext context) : base (context)
        {
            _context = context;
        }
    }
}
