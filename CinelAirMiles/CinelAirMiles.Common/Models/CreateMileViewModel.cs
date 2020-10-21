namespace CinelAirMiles.Common.Models
{
    using System;
    using System.Collections.Generic;
    using CinelAirMiles.Common.Entities;

    public class CreateMileViewModel
    {
        public int Miles { get; set; }

        public MilesType MilesType { get; set; }

        //public IEnumerable<SelectListItem> MilesType { get; set; }

        public DateTime CreditDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string MilesProgramNumber { get; set; }
    }
}