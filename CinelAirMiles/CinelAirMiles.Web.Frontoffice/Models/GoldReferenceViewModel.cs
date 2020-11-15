using CinelAirMiles.Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CinelAirMiles.Web.Frontoffice.Models
{
    public class GoldReferenceViewModel
    {
        public string Info { get; set; }


        public int ViewState { get; set; }


        public string ReferrerClientNumber { get; set; }


        [StringLength(9, MinimumLength = 9, ErrorMessage = "Must be a 9 digit number")]
        [Display(Name = "The client number to which you desire to share your Gold tier")]
        public string ReferredClientNumber { get; set; }
    }
}
