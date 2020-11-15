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
    public class ComplaintRepository : GenericRepository<Complaint>, IComplaintRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public ComplaintRepository(
            ApplicationDbContext context,
            UserManager<User> userManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
        }

        async Task RequestNewComplaintAsync(Complaint newComplaint, User user)
        {
            var notification = new Notification
            {
                Text = $"Client with email {user.UserName} has requested a complaint with subject: {newComplaint.Subject}."
            };

            await _context.ComplaintTemps.AddAsync(
                new ComplaintTemp
                {
                    Subject = newComplaint.Subject,
                    Description = newComplaint.Description,
                    ComplaintDate = newComplaint.ComplaintDate,
                    MilesProgramNumber = newComplaint.MilesProgramNumber
                });

            await _context.SaveChangesAsync();

            var tempTable = await _context.ComplaintTemps.FirstOrDefaultAsync(cm => cm.Description == newComplaint.Description);

            await CreateNotificationWithPartnerAsync(notification, user.Id, tempTable.Id, "Complaint");
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

        public async Task<string> CreateComplaintAsync(Complaint newComplaint, User user)
        {
            var pendingComplaintAddition = _context.ComplaintTemps.Any(pt => pt.Description == newComplaint.Description);

            if (pendingComplaintAddition == false)
            {
                await RequestNewComplaintAsync(newComplaint, user);

                return "Complaint was sent for evaluation.";
            }
            else
            {
                return "Complaint creation was not requested because there is already a pending complaint with that description.";
            }
        }


        //public async Task<int> GetComplaintsCountAsync()
        //{
        //    return await _context.Complaints
        //        .Where(c => c.Id > 0).CountAsync();
        //}
    }
}
