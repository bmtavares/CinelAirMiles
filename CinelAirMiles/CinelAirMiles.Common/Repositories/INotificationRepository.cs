using CinelAirMiles.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CinelAirMiles.Common.Repositories
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        //Task CreateNotificationWithUserAndTypeAsync(Notification notification, string userId, string notificationType);

        Task<List<NotificationUser>> GetUserNotificationsAsync(string userId);

        Task ReadNotificationAsync(int id);

        Task<string> AcceptAlertAsync(int id);

        Task DenyTierChangeAsync(int id);
    }
}
