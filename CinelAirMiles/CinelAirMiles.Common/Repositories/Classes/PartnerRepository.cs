namespace CinelAirMiles.Common.Repositories.Classes
{
    using System.Linq;
    using System.Threading.Tasks;

    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;
    using CinelAirMiles.Common.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class PartnerRepository : GenericRepository<Partner>, IPartnerRepository
    {
        private readonly ApplicationDbContext _context;
        readonly UserManager<User> _userManager;

        public PartnerRepository(
            ApplicationDbContext context,
            UserManager<User> userManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<int> GetPartnerCountAsync()
        {
            return await _context.Partners.CountAsync();
        }

        public async Task<int> GetBenefitsCountAsync()
        {
            return await _context.Benefits.CountAsync();
        }

        public async Task<string> CreatePartnerAsync(Partner newPartner, User user)
        {
            var pendingPartnerAddition = _context.AddPartnersTemp.Any(pt => pt.Name == newPartner.Name);

            if (pendingPartnerAddition == false)
            {
                await RequestNewPartnerAsync(newPartner, user);

                return "Partner creation was requested to a Super user.";
            }
            else
            {
                return "Partner creation was not requested because there is already a pending request with the same name.";
            }
        }

        async Task RequestNewPartnerAsync(Partner newPartner, User user)
        {
            var notification = new Notification
            {
                Text = $"User {user.UserName} has requested a partner creation for {newPartner.Name}."
            };

            await _context.AddPartnersTemp.AddAsync(
                new AddPartnerTemp
                {
                    Name = newPartner.Name,
                    Description = newPartner.Description
                });

            await _context.SaveChangesAsync();

            var tempTable = await _context.AddPartnersTemp.FirstOrDefaultAsync(pt => pt.Name == newPartner.Name);

            await CreateNotificationWithPartnerAsync(notification, user.Id, tempTable.Id, "PartnerReference");
        }

        async Task CreateNotificationWithPartnerAsync(Notification notification, string userId, int tempTableId, string notificationType)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                //TODO: Proper error treatment
                return;
            }

            var type = await _context.NotificationsTypes.FirstOrDefaultAsync(nt => nt.Type == notificationType);

            if (type == null)
            {
                //TODO: Proper error treatment
                return;
            }

            notification.NotificationType = type;
            notification.TempTableId = tempTableId;

            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();

            var superUsers = await _userManager.GetUsersInRoleAsync("SuperUser");

            foreach (var superUser in superUsers)
            {
                await _context.NotificationsUsers.AddAsync(new NotificationUser
                {
                    Notification = notification,
                    User = superUser
                });
            }

            await _context.SaveChangesAsync();
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
