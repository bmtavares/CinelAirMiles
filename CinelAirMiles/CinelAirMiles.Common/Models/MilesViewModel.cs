using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CinelAirMiles.Common.Models
{
    public class MilesViewModel
    {
        [Display(Name = "Available miles")]
        public int Miles { get; set; }


        [Display(Name = "Credit date")]
        public string CreditDate { get; set; }


        [Display(Name = "Expiry date")]
        public string ExpiryDate { get; set; }


        public string Description { get; set; }
    }
}
