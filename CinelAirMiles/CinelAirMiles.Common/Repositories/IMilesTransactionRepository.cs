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
        /// Transfers the mile balance to the heir client from a deceased client.
        /// </summary>
        /// <param name="heirClient">Receiving Client</param>
        /// <param name="bonusBalance">Bonus balance ammount</param>
        /// <param name="statusBalance">Status balance ammount</param>
        /// <returns>Results from individual transactions</returns>
        Task<string> InheritMilesAsync(Client heirClient, int bonusBalance, int statusBalance);

        /// <summary>
        /// Receives the amount of Bonus Miles to convert to Status, the client to receive them, and checks if the client has enough Bonus Miles balance, if it does, creates a new Mile entity with Status type and and relates it to the received client, while removing the same amount from the relate client's Bonus Miles closest to expiring
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        Task<string> ConvertMilesAsync(int quantity, Client client);
    }
}
