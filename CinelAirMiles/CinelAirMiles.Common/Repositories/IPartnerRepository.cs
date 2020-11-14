namespace CinelAirMiles.Common.Repositories
{

    using System.Threading.Tasks;

    using CinelAirMiles.Common.Entities;
    using CinelAirMiles.Common.Models;

    public interface IPartnerRepository : IGenericRepository<Partner>
    {
        Task<string> CreatePartnerAsync(Partner newPartner, User user);

        Task<Partner> GetParnerWithBenefitsAsync(int id);

        Task AddBenefitAsync(CreateBenefitViewModel model);


        Task<Benefit> GetBenefitAsync(int id);

        Task<int> UpdateBenefitAsync(Benefit benefit);

        Task<int> DeleteBenefitAsync(Benefit benefit);

    }
}
