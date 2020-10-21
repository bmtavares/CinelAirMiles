namespace CinelAirMiles.Common.Models
{
    using System.ComponentModel.DataAnnotations;

    public class MembershipLoginViewModel
    {
        [Required]
        public string ProgramNumber { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }


        public bool RememberMe { get; set; }
    }
}
