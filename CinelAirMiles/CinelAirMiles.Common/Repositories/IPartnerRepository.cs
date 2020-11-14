using CinelAirMiles.Common.Entities;
using CinelAirMiles.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CinelAirMiles.Common.Repositories
{
    public interface IPartnerRepository : IGenericRepository<Partner>
    {

        Task<Partner> GetParnerWithBenefitsAsync(int id);

        Task AddBenefitAsync(CreateBenefitViewModel model);


        Task<Benefit> GetBenefitAsync(int id);

        Task<int> UpdateBenefitAsync(Benefit benefit);

        Task<int> DeleteBenefitAsync(Benefit benefit);

    }
}
