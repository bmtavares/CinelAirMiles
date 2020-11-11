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
        //readonly UserManager<User> _userManager;

        public NotificationRepository(
            ApplicationDbContext context /*,
            UserManager<User> userManager*/) : base(context)
        {
            _context = context;
            //_userManager = userManager;
        }

        public async Task AcceptAlertAsync(int id)
        {
            var notification = await _context.Notifications
                .Include(n => n.NotificationType)
                .FirstOrDefaultAsync(n => n.Id == id);

            var notificationType = notification.NotificationType.Type;

            switch (notificationType)
            {
                case "TierChange":
                    var tempTable = await _context.ChangeClientsTierTemp
                        .Include(cc => cc.Client)
                        .ThenInclude(c => c.ProgramTier)
                        .Include(cc => cc.ProgramTier)
                        .FirstOrDefaultAsync(cc => cc.Id == notification.TempTableId);

                    var client = tempTable.Client;
                    var programTier = tempTable.ProgramTier;

                    client.ProgramTier = programTier;

                    _context.Clients.Update(client);
                    _context.ChangeClientsTierTemp.Remove(tempTable);
                    await _context.SaveChangesAsync();
                    break;

                case "Complaint":
                    //TODO Pending
                    break;

                case "SeatAvailability":
                    //TODO Pending
                    break;

                case "PartnerReference":
                    //TODO Pending
                    break;

                case "AdInsertion":
                    //TODO Pending
                    break;
            }
        }

        //public async Task DenyTierChangeAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}


        //public async Task CreateNotificationWithUserAndTypeAsync(Notification notification, string userId, string notificationType)
        //{
        //    var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        //    if(user == null)
        //    {
        //        //TODO: Proper error treatment
        //        return;
        //    }

        //    var type = await _context.NotificationsTypes.FirstOrDefaultAsync(nt => nt.Type == notificationType);

        //    if(type == null)
        //    {
        //        //TODO: Proper error treatment
        //        return;
        //    }

        //    notification.NotificationType = type;

        //    await CreateAsync(notification);

        //    var superUsers = await _userManager.GetUsersInRoleAsync("SuperUser");

        //    var notificationUser = new NotificationUser
        //    {
        //        Notification = notification
        //    };

        //    foreach(var superUser in superUsers)
        //    {
        //        notificationUser.User = superUser;
        //        await _context.NotificationsUsers.AddAsync(notificationUser);
        //    }
        //}

        public async Task<List<NotificationUser>> GetUserNotificationsAsync(string userId)
        {
            return await _context.NotificationsUsers.Where(nu => nu.UserId == userId)
                .Include(nu => nu.Notification)
                .Where(nu => nu.Notification.IsRead == false)
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
