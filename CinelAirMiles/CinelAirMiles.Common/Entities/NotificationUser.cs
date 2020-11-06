using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CinelAirMiles.Common.Entities
{
    public class NotificationUser : IEntity
    {
        public int Id { get; set; }


        [Required]
        public string UserId { get; set; }

        
        [Required]
        public User User { get; set; }


        [Required]
        public Notification Notification { get; set; }
    }
}
