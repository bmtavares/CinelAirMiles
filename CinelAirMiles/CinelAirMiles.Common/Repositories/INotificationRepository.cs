using CinelAirMiles.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CinelAirMiles.Common.Repositories
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        /// <summary>
        /// Returns a list of notifications associated with the indicated SuperUser
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<NotificationUser>> GetUserNotificationsAsync(string userId);

        /// <summary>
        /// Turns the IsRead boolean value to true of the notification with the specified Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task ReadNotificationAsync(int id);

        /// <summary>
        /// Accepts the notification with the received Id by removing the appropriate values from the their respective temporary table and inserts them into the corresponding definitive table
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> AcceptAlertAsync(int id);

        /// <summary>
        /// Denies the notification with the received Id by purging the appropriate values from the their respective temporary table and consequently from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DenyAlertAsync(int id);
    }
}
