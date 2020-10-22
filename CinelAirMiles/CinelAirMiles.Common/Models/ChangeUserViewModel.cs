namespace CinelAirMiles.Common.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ChangeUserViewModel
    {
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }


        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }



        [MaxLength(20, ErrorMessage = "The field {0} only can contain {1} characters.")]
        public string PhoneNumber { get; set; }
    }
}
