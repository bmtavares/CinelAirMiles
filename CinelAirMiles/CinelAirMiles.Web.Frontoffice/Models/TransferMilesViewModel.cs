using CinelAirMiles.Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CinelAirMiles.Web.Frontoffice.Models
{
    public class TransferMilesViewModel : PurchaseMilesViewModel
    {
        [Required]
        [Display(Name = "Receiving client number")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "Insert nine digits for the client number")]
        public string ClientToTransferToNumber { get; set; }
    }
}
