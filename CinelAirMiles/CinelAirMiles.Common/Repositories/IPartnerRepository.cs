using CinelAirMiles.Common.Entities;
using CinelAirMiles.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
