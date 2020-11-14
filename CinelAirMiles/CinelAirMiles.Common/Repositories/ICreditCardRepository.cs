namespace CinelAirMiles.Common.Repositories
{
    using CinelAirMiles.Common.Entities;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ICreditCardRepository : IGenericRepository<CreditCardInfo>
    {
        /// <summary>
        /// Receives a credit card, and if it doesn't exist in the context, it gets added
        /// </summary>
        /// <param name="creditCard"></param>
        /// <returns></returns>
        Task CheckExistingCreditCardByNumberAsync(CreditCardInfo creditCard);

        /// <summary>
        /// Returns all credit cards associated with a client
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        IEnumerable<CreditCardInfo> GetCreditCardsAssociatedWithClient(Client client);
    }
}
