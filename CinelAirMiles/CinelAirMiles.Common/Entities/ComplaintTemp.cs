using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CinelAirMiles.Common.Entities
{
    public class ComplaintTemp : IEntity
    {
        public int Id { get; set; }

        public string MilesProgramNumber { get; set; }


        public DateTime ComplaintDate { get; set; }


        public string Subject { get; set; }


        public string Description { get; set; }
    }
}
