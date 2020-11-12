using System;
using System.Collections.Generic;
using System.Text;

namespace CinelAirMiles.Common.Entities
{
    public class Partner : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<Benefit> Benefits { get; set; }
    }
}
