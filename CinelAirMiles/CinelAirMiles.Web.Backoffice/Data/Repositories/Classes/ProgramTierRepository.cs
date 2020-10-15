using CinelAirMiles.Common.Entities;
using CinelAirMiles.Web.Backoffice.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinelAirMiles.Web.Backoffice.Data.Repositories.Classes
{
    public class ProgramTierRepository : GenericRepository<ProgramTier>, IProgramTierRepository
    {
        private readonly ApplicationDbContext _context;

        public ProgramTierRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
