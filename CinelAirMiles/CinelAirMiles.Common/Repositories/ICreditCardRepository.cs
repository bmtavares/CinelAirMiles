namespace CinelAirMiles.Common.Repositories
{
    using CinelAirMiles.Common.Entities;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ICreditCardRepository : IGenericRepository<CreditCardInfo>
    {
        Task CheckExistingCreditCardByNumberAsync(CreditCardInfo creditCard);

        IEnumerable<CreditCardInfo> GetCreditCardsAssociatedWithClient(Client client);
    }
}
