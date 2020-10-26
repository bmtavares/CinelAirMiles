using CinelAirMiles.Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CinelAirMiles.Web.Backoffice.Models
{
    public class UserViewModel
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


        public string Email { get; set; }


        //TOOO: Make this prop required
        //[Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
