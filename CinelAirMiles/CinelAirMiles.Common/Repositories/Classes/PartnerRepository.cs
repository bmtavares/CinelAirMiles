namespace CinelAirMiles.Common.Repositories.Classes
{
    using System.Linq;
    using System.Threading.Tasks;

    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;

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
    }
}
