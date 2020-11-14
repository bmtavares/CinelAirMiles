namespace CinelAirMiles.Common.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "The field {0} must be at least {1} characters long.")]
        public string Password { get; set; }


        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
