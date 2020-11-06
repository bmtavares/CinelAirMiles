using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinelAirMiles.Common.Repositories;
using CinelAirMiles.Web.Backoffice.Helpers.Classes;
using CinelAirMiles.Web.Backoffice.Helpers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinelAirMiles.Web.Backoffice.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        readonly INotificationRepository _notificationRepository;
        readonly IUserHelper _userHelper;

        public NotificationsController(
            INotificationRepository notificationRepository,
            IUserHelper userHelper)
        {
            _userHelper = userHelper;
            _notificationRepository = notificationRepository;
        }


        public async Task<IActionResult> GetNotifications()
        {
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            var notifications = await _notificationRepository.GetUserNotificationsAsync(user.Id);

            return Ok(new { UserNotifications = notifications, Count = notifications.Count });
        }

        public async Task<IActionResult> ReadNotification(int notificationId)
        {
            await _notificationRepository.ReadNotificationAsync(notificationId);

            return Ok();
        }
    }
}
