using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CinelAirMiles.Web.Backoffice.Data.Entities
{
    public class ReferrerProgram
    {
        public Client ReferrerClient { get; set; }


        public int ReferrerClientId { get; set; }


        public Client ReferredClient { get; set; }


        public int ReferredClientId { get; set; }
    }
}
