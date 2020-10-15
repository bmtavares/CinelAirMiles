using CinelAirMiles.Web.Backoffice.Data.Entities;
using CinelAirMiles.Web.Backoffice.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinelAirMiles.Web.Backoffice.Data.Repositories.Classes
{
    public class MilesTransactionRepository: GenericRepository<MilesTransaction>, IMilesTransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public MilesTransactionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
