namespace CinelAirMiles.Common.Entities
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }


        public string MainRole { get; set; }

    }
}
