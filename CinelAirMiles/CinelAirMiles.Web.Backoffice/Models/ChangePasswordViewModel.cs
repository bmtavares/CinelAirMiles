using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CinelAirMiles.Web.Backoffice.Models
{
    public class ChangePasswordViewModel
    {
        [Required]
        [Display(Name = "Current password")]
        public string CurrentPassword { get; set; }

        [Required]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
