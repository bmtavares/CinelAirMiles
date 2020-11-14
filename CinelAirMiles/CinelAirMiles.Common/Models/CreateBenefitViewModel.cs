
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CinelAirMiles.Common.Models
{
    public class CreateBenefitViewModel
    {
        
        public int PartnerId { get; set; }

        public int BenefitId { get; set; }

        public string Description { get; set; }

        public float Reward { get; set; }

        public bool IsPercentage { get; set; }
    }
}
