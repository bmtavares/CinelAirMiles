namespace CinelAirMiles.Common.Repositories.Classes
{
    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;
    using CinelAirMiles.Common.Repositories;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading.Tasks;

    public class MilesTransactionRepository : GenericRepository<MilesTransaction>, IMilesTransactionRepository
    {
        readonly ApplicationDbContext _context;

        public MilesTransactionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task PurchaseMilesAsync(int quantity, Client client, CreditCardInfo cardInfo)
        {
            var type = await _context.MilesTypes.FirstOrDefaultAsync(mt => mt.Description == "Bonus");

            if(type == null)
            {
                return;
            }

            var mile = new Mile
            {
                Miles = quantity,
                Client = client,
                CreditDate = DateTime.UtcNow,
                MilesType = type,
                ExpiryDate = DateTime.UtcNow.AddYears(1),
                Description = "Purchased miles"
            };

            var transactionType = await _context.MilesTransactionTypes.FirstOrDefaultAsync(mtt => mtt.Description == "Purchase");

            if(transactionType == null)
            {
                return;
            }

            var transaction = new MilesTransaction
            {
                Client = client,
                Mile = mile,
                TransactionDate = DateTime.UtcNow,
                MilesTransactionType = transactionType
            };

            await _context.Miles.AddAsync(mile);
            await CreateAsync(transaction);
        }
    }
}
