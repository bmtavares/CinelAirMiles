using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CinelAirMiles.Common.Entities
{
    public class Notification : IEntity
    {
        public int Id { get; set; }


        [Required]
        public string Text { get; set; }


        [Required]
        public bool IsRead { get; set; } = false;


        public int TempTableId { get; set; }


        [Required]
        public NotificationType NotificationType { get; set; }


        public List<NotificationUser> NotificationUsers { get; set; }
    }
}
