namespace CinelAirMiles.Web.Backoffice.Models
{
    using System;
    using System.Collections.Generic;
    using CinelAirMiles.Common.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CreateMileViewModel
    {
        public int Miles { get; set; }

        public IEnumerable<SelectListItem> MilesType { get; set; }

        public int MilesTypeId { get; set; }

        public DateTime CreditDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string MilesProgramNumber { get; set; }
    }
}