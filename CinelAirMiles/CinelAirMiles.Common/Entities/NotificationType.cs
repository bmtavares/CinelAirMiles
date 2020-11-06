using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CinelAirMiles.Common.Entities
{
    public class NotificationType : IEntity
    {
        public int Id { get; set; }


        [Required]
        public string Type { get; set; }
    }
}
