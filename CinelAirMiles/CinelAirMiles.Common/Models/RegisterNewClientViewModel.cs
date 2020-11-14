using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CinelAirMiles.Common.Models
{
    public class RegisterNewClientViewModel : RegisterNewUserViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Birth date")]
        public DateTime? BirthDate { get; set; }
    }
}
