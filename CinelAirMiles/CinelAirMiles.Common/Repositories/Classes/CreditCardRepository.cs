namespace CinelAirMiles.Common.Repositories.Classes
{
    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;

    public class CreditCardRepository : GenericRepository<CreditCardInfo>, ICreditCardRepository
    {

        public CreditCardRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
