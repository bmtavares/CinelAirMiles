using CinelAirMiles.Common.Data;
using CinelAirMiles.Common.Entities;
using CinelAirMiles.Common.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinelAirMiles.Common.Repositories.Classes
{
    public class PartnerRepository : GenericRepository<Partner>, IPartnerRepository
    {
        private readonly ApplicationDbContext _context;

        public PartnerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

       
        public async Task AddBenefitAsync(CreateBenefitViewModel model)
        {
            var partner = await this.GetParnerWithBenefitsAsync(model.PartnerId);
            if (partner == null)
            {
                return;
            }

            partner.Benefits.Add(new Benefit { Description = model.Description, Reward = model.Reward, IsPercentage = model.IsPercentage });
            _context.Partners.Update(partner);
            await _context.SaveChangesAsync();
        }

        
        public async Task<int> DeleteBenefitAsync(Benefit benefit)
        {
            var partner = await _context.Partners.Where(b => b.Benefits.Any(be => be.Id == benefit.Id)).FirstOrDefaultAsync();
            if (partner == null)
            {
                return 0;
            }

            _context.Benefits.Remove(benefit);
            await _context.SaveChangesAsync();
            return partner.Id;
        }


        public async Task<Benefit> GetBenefitAsync(int id)
        {
            return await _context.Benefits.FindAsync(id);
        }


        public async Task<Partner> GetParnerWithBenefitsAsync(int id)
        {
            return await _context.Partners
             .Include(c => c.Benefits)
             .Where(c => c.Id == id)
             .FirstOrDefaultAsync();

        }


        public async Task<int> UpdateBenefitAsync(Benefit benefit)
        {
            var partner = await _context.Partners.Where(b => b.Benefits.Any(be => be.Id == benefit.Id)).FirstOrDefaultAsync();
            if (partner == null)
            {
                return 0;
            }

            _context.Benefits.Update(benefit);
            await _context.SaveChangesAsync();
            return partner.Id;
        }
    }
}
