namespace CinelAirMiles.Common.Models
{
    using System.ComponentModel.DataAnnotations;

    public class EditUserViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [MaxLength(75, ErrorMessage = "The field {0} cannot exceed {1} characters.")]
        public string FirstName { get; set; }


        [Required]
        [Display(Name = "Last Name")]
        [MaxLength(75, ErrorMessage = "The field {0} cannot exceed {1} characters.")]
        public string LastName { get; set; }


        [MaxLength(20, ErrorMessage = "The field {0} only can contain {1} characters.")]
        public string PhoneNumber { get; set; }
    }
}
