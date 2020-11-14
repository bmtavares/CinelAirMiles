namespace CinelAirMiles.Common.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterNewUserViewModel
    {
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }


        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }


        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters.")]
        public string Address { get; set; }


        [MaxLength(20, ErrorMessage = "The field {0} only can contain {1} characters.")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }


        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }


        [Required]
        public string Password { get; set; }


        [Required]
        [Compare("Password")]
        [Display(Name = "Confirm password")]
        public string Confirm { get; set; }
    }
}
