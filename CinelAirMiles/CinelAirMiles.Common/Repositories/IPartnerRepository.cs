namespace CinelAirMiles.Common.Repositories
{

    using System.Threading.Tasks;

    using CinelAirMiles.Common.Entities;
    using CinelAirMiles.Common.Models;

    public interface IPartnerRepository : IGenericRepository<Partner>
    {
        /// <summary>
        /// Returns partners count
        /// </summary>
        /// <returns>Partners count</returns>
        Task<int> GetPartnerCountAsync();

        /// <summary>
        /// Returns benefits count
        /// </summary>
        /// <returns>Benefits count</returns>
        Task<int> GetBenefitsCountAsync();

        Task<string> CreatePartnerAsync(Partner newPartner, User user);

        Task<Partner> GetParnerWithBenefitsAsync(int id);

        Task AddBenefitAsync(CreateBenefitViewModel model);


        Task<Benefit> GetBenefitAsync(int id);

        Task<int> UpdateBenefitAsync(Benefit benefit);

        Task<int> DeleteBenefitAsync(Benefit benefit);

    }
}
