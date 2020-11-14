namespace CinelAirMiles.Common.Repositories
{
    using CinelAirMiles.Common.Entities;
    using System.Threading.Tasks;

    public interface IMilesTransactionRepository : IGenericRepository<MilesTransaction>
    {
        /// <summary>
        /// Receives the amount of miles, and the client for the Bonus Miles balance purchase
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        Task<string> PurchaseMilesAsync(int quantity, Client client);

        /// <summary>
        /// Receives the amount of miles, and the both clients on the giving and receiving ends for the Bonus Miles balance transfer
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="receivingClient"></param>
        /// <param name="Transferringclient"></param>
        /// <returns></returns>
        Task<string> TransferMilesAsync(int quantity, Client receivingClient, Client Transferringclient);

        /// <summary>
        /// Receives the amount of Bonus Miles to convert to Status, and its respective client
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        Task<string> ConvertMilesAsync(int quantity, Client client);

        //Task<string> ExtendMilesAsync(Mile mile, Client client);
    }
}
