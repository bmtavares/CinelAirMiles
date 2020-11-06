using CinelAirMiles.Common.Data;
using CinelAirMiles.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinelAirMiles.Common.Repositories.Classes
{
    public class ContactFormRepository : GenericRepository<ContactForm>, IContactFormRepository
    {
        readonly ApplicationDbContext _context;

        public ContactFormRepository(
            ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
