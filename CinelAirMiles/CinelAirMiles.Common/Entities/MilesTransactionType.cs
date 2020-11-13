using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CinelAirMiles.Common.Entities
{
    public class MilesTransactionType : IEntity
    {
        public int Id { get; set; }


        [Display(Name = "Type of transaction")]
        public string Description { get; set; }
    }
}
