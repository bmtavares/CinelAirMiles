using CinelAirMiles.Common.Data;
using CinelAirMiles.Common.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinelAirMiles.Common.Repositories.Classes
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        readonly ApplicationDbContext _context;
        readonly UserManager<User> _userManager;

        public NotificationRepository(
            ApplicationDbContext context,
            UserManager<User> userManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task CreateNotificationWithUserAndTypeAsync(Notification notification, string userId, string notificationType)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if(user == null)
            {
                //TODO: Proper error treatment
                return;
            }

            var type = await _context.NotificationsTypes.FirstOrDefaultAsync(nt => nt.Type == notificationType);

            if(type == null)
            {
                //TODO: Proper error treatment
                return;
            }

            notification.NotificationType = type;

            await CreateAsync(notification);

            var superUsers = await _userManager.GetUsersInRoleAsync("SuperUser");

            var notificationUser = new NotificationUser
            {
                Notification = notification
            };

            foreach(var superUser in superUsers)
            {
                notificationUser.User = superUser;
                await _context.NotificationsUsers.AddAsync(notificationUser);
            }
        }

        public async Task<List<NotificationUser>> GetUserNotificationsAsync(string userId)
        {
            return await _context.NotificationsUsers.Where(nu => nu.UserId == userId)
                .Include(nu => nu.Notification)
                .ToListAsync();
        }

        public async Task ReadNotificationAsync(int id)
        {
            var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);

            notification.IsRead = true;

            await UpdateAsync(notification);
        }
    }
}
