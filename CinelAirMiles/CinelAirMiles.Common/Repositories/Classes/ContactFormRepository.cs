namespace CinelAirMiles.Common.Repositories.Classes
{
    using System.Threading.Tasks;

    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;

    using Microsoft.EntityFrameworkCore;

    public class ContactFormRepository : GenericRepository<ContactForm>, IContactFormRepository
    {
        readonly ApplicationDbContext _context;

        public ContactFormRepository(
            ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.ContactForms.CountAsync();
        }
    }
}
