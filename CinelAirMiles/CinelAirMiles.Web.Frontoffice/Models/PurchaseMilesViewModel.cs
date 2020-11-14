using CinelAirMiles.Common.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CinelAirMiles.Web.Frontoffice.Models
{
    public class PurchaseMilesViewModel
    {
        [Required]
        [Display(Name = "Amount of Bonus Miles for transaction")]
        public int Quantity { get; set; }


        [Display(Name = "Price")]
        public decimal ValueToPay { get; set; }


        //[Display(Name = "Credit card")]
        //[Range(1, int.MaxValue, ErrorMessage = "You must select a credit card")]
        //public int CreditCardInfoId { get; set; }


        public CreditCardInfo CreditCardInfo { get; set; }


        //public IEnumerable<SelectListItem> CreditCards { get; set; }
    }
}
