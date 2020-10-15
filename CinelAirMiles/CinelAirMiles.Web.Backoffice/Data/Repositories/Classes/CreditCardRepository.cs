using CinelAirMiles.Web.Backoffice.Data.Entities;
using CinelAirMiles.Web.Backoffice.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinelAirMiles.Web.Backoffice.Data.Repositories.Classes
{
    public class CreditCardRepository : GenericRepository<CreditCardInfo>, ICreditCardRepository
    {
        private readonly ApplicationDbContext _context;

        public CreditCardRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
