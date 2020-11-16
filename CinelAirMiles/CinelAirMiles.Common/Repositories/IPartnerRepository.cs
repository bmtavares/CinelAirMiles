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

        /// <summary>
        /// Creates a new partner
        /// </summary>
        /// <param name="newPartner"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> CreatePartnerAsync(Partner newPartner, User user);

        /// <summary>
        /// Returns Partners
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Partner> GetParnerWithBenefitsAsync(int id);

        /// <summary>
        /// Creates a new Benefit
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task AddBenefitAsync(CreateBenefitViewModel model);

        /// <summary>
        /// Returns Benefit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Benefit> GetBenefitAsync(int id);

        /// <summary>
        /// Updates Benefit
        /// </summary>
        /// <param name="benefit"></param>
        /// <returns></returns>
        Task<int> UpdateBenefitAsync(Benefit benefit);

        /// <summary>
        /// Deletes Benefit
        /// </summary>
        /// <param name="benefit"></param>
        /// <returns></returns>
        Task<int> DeleteBenefitAsync(Benefit benefit);

    }
}
