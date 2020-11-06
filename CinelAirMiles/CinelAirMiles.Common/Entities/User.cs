namespace CinelAirMiles.Common.Entities
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }


        public string MainRole { get; set; }


        public bool RequirePasswordChange { get; set; }

        public List<NotificationUser> NotificationUsers { get; set; }
    }
}
