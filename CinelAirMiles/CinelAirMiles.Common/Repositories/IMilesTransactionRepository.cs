namespace CinelAirMiles.Common.Repositories
{
    using CinelAirMiles.Common.Entities;
    using System.Threading.Tasks;

    public interface IMilesTransactionRepository : IGenericRepository<MilesTransaction>
    {
        Task PurchaseMilesAsync(int quantity, Client client, CreditCardInfo cardInfo);
    }
}
