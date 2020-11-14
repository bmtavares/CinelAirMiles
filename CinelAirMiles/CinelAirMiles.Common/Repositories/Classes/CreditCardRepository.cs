namespace CinelAirMiles.Common.Repositories.Classes
{
    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CreditCardRepository : GenericRepository<CreditCardInfo>, ICreditCardRepository
    {
        readonly ApplicationDbContext _context;

        public CreditCardRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task CheckExistingCreditCardByNumberAsync(CreditCardInfo creditCard)
        {
            var existingCard = await _context.CreditCardsInfo.FirstOrDefaultAsync(cc => cc.Number == creditCard.Number);

            if (existingCard == null)
            {
                await CreateAsync(creditCard);

                var creditCardWithId = await _context.CreditCardsInfo.FirstOrDefaultAsync(cc => cc.Number == creditCard.Number);
            }
        }

        public IEnumerable<CreditCardInfo> GetCreditCardsAssociatedWithClient(Client client)
            => _context.CreditCardsInfo.Where(cc => cc.Client == client);
    }
}
