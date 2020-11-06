using System;
using System.Collections.Generic;
using System.Text;

namespace CinelAirMiles.Common.Entities
{
    public class ChangeClientTierTemp : IEntity
    {
        public int Id { get; set; }


        public Client Client { get; set; }


        public bool? ChangeConfirmed { get; set; } = null;
    }
}
