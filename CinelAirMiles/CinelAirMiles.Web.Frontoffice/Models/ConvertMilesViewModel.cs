using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CinelAirMiles.Web.Frontoffice.Models
{
    public class ConvertMilesViewModel : PurchaseMilesViewModel
    {
        [Required]
        [Display(Name = "Status Miles to be received")]
        public int StatusMilesToReceive { get; set; }
    }
}
