using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CinelAirMiles.Common.Entities
{
    public class Complaint : IEntity
    {
        public int Id { get; set; }


        [Display(Name = "Client number")]
        public string MilesProgramNumber { get; set; }


        [Display(Name = "Complaint date")]
        public DateTime ComplaintDate { get; set; }


        public string Subject { get; set; }


        public string Description { get; set; }
    }
}
