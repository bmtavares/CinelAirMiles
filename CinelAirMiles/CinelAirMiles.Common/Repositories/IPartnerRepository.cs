namespace CinelAirMiles.Common.Repositories
{
    using System.Threading.Tasks;

    using CinelAirMiles.Common.Entities;

    public interface IPartnerRepository : IGenericRepository<Partner>
    {
        Task<string> CreatePartnerAsync(Partner newPartner, User user);
    }
}
