using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinelAirMiles.Common.Entities;
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

        public async Task<IActionResult> Index()
        {
            var notificationsUsers = await GetUsersNotificationsAsync();
            var notifications = notificationsUsers.Select(nt => nt.Notification).Where(nt => nt.IsRead == false);

            return View(notifications);
        }

        public async Task<IActionResult> AcceptAlert(int? id)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            await ReadNotification(id.Value);

            //TODO Show this message in the index view
            var message = await _notificationRepository.AcceptAlertAsync(id.Value);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DenyAlert(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            await ReadNotification(id.Value);

            await _notificationRepository.DenyAlertAsync(id.Value);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetNotifications()
        {
            var notifications = await GetUsersNotificationsAsync();

            return Ok(new { UserNotifications = notifications, Count = notifications.Count });
        }

        public async Task<IActionResult> ReadNotification(int notificationId)
        {
            await _notificationRepository.ReadNotificationAsync(notificationId);

            return Ok();
        }

        async Task<List<NotificationUser>> GetUsersNotificationsAsync()
        {
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            var notifications = await _notificationRepository.GetUserNotificationsAsync(user.Id);

            return notifications;
        }
    }
}
