namespace CinelAirMiles.Common.Repositories.Classes
{
    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;
    using CinelAirMiles.Common.Repositories;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class MilesTransactionRepository : GenericRepository<MilesTransaction>, IMilesTransactionRepository
    {
        readonly ApplicationDbContext _context;

        public MilesTransactionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<string> PurchaseMilesAsync(int quantity, Client client) =>
            await ExecuteTransactionAsync(quantity, client, "Purchase", 1, "Purchased miles", "Bonus");

        public async Task<string> TransferMilesAsync(int quantity, Client receivingClient, Client Transferringclient)
        {
            var mileType = await _context.MilesTypes.FirstOrDefaultAsync(mt => mt.Description == "Bonus");

            var milesAssociatedWithTransferringClient = _context.Miles
                .Where(m => m.Client == Transferringclient && m.ExpiryDate >= DateTime.UtcNow && m.MilesType == mileType)
                .OrderByDescending(m => m.ExpiryDate)
                .ToList();

            int checkIfClientHasEnoughBalance = 0;

            foreach (var clientsMile in milesAssociatedWithTransferringClient)
            {
                checkIfClientHasEnoughBalance += clientsMile.Balance;
            }

            if(checkIfClientHasEnoughBalance < quantity)
            {
                return "You don't have enough Bonus Miles to perform this operation";
            }

            int valueToCompare = 0;
            int i = 0;

            while (valueToCompare < quantity)
            {
                if (milesAssociatedWithTransferringClient[i].Balance > 0)
                {
                    milesAssociatedWithTransferringClient[i].Balance--;
                    valueToCompare++;
                }
                else
                {
                    _context.Miles.Update(milesAssociatedWithTransferringClient[i]);
                    await _context.SaveChangesAsync();
                    i++;
                }
            }

            return await ExecuteTransactionAsync(quantity, receivingClient, "Transfer", 1, $"Miles transfered from client {Transferringclient.MilesProgramNumber}", "Bonus");
        }

        public async Task<string> ConvertMilesAsync(int quantity, Client client)
        {
            var mileType = await _context.MilesTypes.FirstOrDefaultAsync(mt => mt.Description == "Bonus");

            var milesAssociatedWithTransferringClient = _context.Miles
                .Where(m => m.Client == client && m.ExpiryDate >= DateTime.UtcNow && m.MilesType == mileType)
                .OrderBy(m => m.ExpiryDate)
                .ToList();

            int checkIfClientHasEnoughBalance = 0;

            foreach (var clientsMile in milesAssociatedWithTransferringClient)
            {
                checkIfClientHasEnoughBalance += clientsMile.Balance;
            }

            if (checkIfClientHasEnoughBalance < quantity)
            {
                return "You don't have enough Bonus Miles to perform this operation";
            }

            int valueToCompare = 0;
            int i = 0;

            while (valueToCompare < quantity)
            {
                if (milesAssociatedWithTransferringClient[i].Balance > 0)
                {
                    milesAssociatedWithTransferringClient[i].Balance--;
                    valueToCompare++;
                }
                else
                {
                    _context.Miles.Update(milesAssociatedWithTransferringClient[i]);
                    await _context.SaveChangesAsync();
                    i++;
                }
            }

            return await ExecuteTransactionAsync(quantity/2, client, "Conversion", 1, $"Miles converted from Bonus to Status", "Status");
        }

        //public async Task<string> ExtendMilesAsync(Mile mile, Client client)
        //{
        //    //await ExecuteTransaction(mile.Miles, client, "Extension", 1);
        //    //TODO not finished
        //}


        /// <summary>
        /// Receives the amount of miles, the client to receive them, and additionally, the type of transaction that has to coincide with an already existing type from the TransactionTypes table, the validity in years of the mile, its description, and the type of mile that also has to coincide with an existing type in the MilesTypes table
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="client"></param>
        /// <param name="typeOfTransaction"></param>
        /// <param name="validityInYears"></param>
        /// <param name="description"></param>
        /// <param name="mileType"></param>
        /// <returns></returns>
        async Task<string> ExecuteTransactionAsync(int quantity, Client client, string typeOfTransaction, int validityInYears, string description, string mileType)
        {
            var type = await _context.MilesTypes.FirstOrDefaultAsync(mt => mt.Description == mileType);

            if (type == null)
            {
                return "An internal error occurred";
            }

            var mile = new Mile
            {
                Miles = quantity,
                Balance = quantity,
                Client = client,
                CreditDate = DateTime.UtcNow,
                MilesType = type,
                ExpiryDate = DateTime.UtcNow.AddYears(validityInYears),
                Description = description
            };

            var transactionType = await _context.MilesTransactionTypes.FirstOrDefaultAsync(mtt => mtt.Description == typeOfTransaction);

            if (transactionType == null)
            {
                return "An internal error occurred";
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

            return "Operation executed successfully";
        }
    }
}
