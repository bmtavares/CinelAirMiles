using CinelAirMiles.Common.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Syncfusion.EJ2.Navigations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CinelAirMiles.Web.Frontoffice.Models
{
    public class PurchaseMilesViewModel
    {
        [Required]
        public int Quantity { get; set; }


        public decimal ValueToPay { get; set; }


        //public int CreditCardInfoId { get; set; }


        public CreditCardInfo CreditCardInfo { get; set; }


        //public IEnumerable<SelectListItem> CreditCards { get; set; }


        //public TabHeader NewCardHeader { get; } = new TabHeader { Text = "Register a new credit card" };


        //public TabHeader ExistingCardHeader { get; } = new TabHeader { Text = "Use an existing credit card" };
    }
}
