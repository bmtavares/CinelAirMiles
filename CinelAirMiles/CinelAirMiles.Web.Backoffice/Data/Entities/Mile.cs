using CinelAirMiles.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinelAirMiles.Web.Backoffice.Data.Entities
{
    public class Mile : IEntity
    {
        public int Id { get; set; }


        public Client Client { get; set; }


        public int Miles { get; set; }


        public MilesType MilesType { get; set; }


        public DateTime CreditDate { get; set; }


        public DateTime ExpiryDate { get; set; }
    }
}
