namespace CinelAirMiles.Common.Models
{
    using System.ComponentModel.DataAnnotations;

    public class MembershipLoginViewModel
    {
        [Required]
        [Display(Name = "Miles program number")]
        public string ProgramNumber { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }


        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
