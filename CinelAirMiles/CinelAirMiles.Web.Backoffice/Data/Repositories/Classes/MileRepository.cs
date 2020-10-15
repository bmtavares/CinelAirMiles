using CinelAirMiles.Web.Backoffice.Data.Entities;
using CinelAirMiles.Web.Backoffice.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinelAirMiles.Web.Backoffice.Data.Repositories.Classes
{
    public class MileRepository : GenericRepository<Mile>, IMileRepository
    {
        private readonly ApplicationDbContext _context;

        public MileRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
