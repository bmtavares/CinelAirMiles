using System;
using System.Collections.Generic;
using System.Text;

namespace CinelAirMiles.Common.Entities
{
    public class Benefit : IEntity
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public float Reward { get; set; }

        public bool IsPercentage { get; set; }

        public Partner Partner { get; set; }
    }
}
