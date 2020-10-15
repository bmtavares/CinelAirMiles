using CinelAirMiles.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinelAirMiles.Web.Backoffice.Data.Entities
{
    public class MilesTransaction : IEntity
    {
        public int Id { get; set; }


        public Mile Mile { get; set; }


        public DateTime TransactionDate { get; set; }
        

        public Client Client { get; set; }
    }
}
