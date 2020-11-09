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
        public DateTime? BirthDate { get; set; }
    }
}
