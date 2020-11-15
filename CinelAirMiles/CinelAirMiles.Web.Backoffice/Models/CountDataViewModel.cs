using CinelAirMiles.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinelAirMiles.Web.Backoffice.Models
{
    public class CountDataViewModel : User
    {
        public int EmployeesCount { get; set; }

        public int ClientCount { get; set; }

        public int PartnerCount { get; set; }

        public int BenefitsCount { get; set; }

        public int SubscriptionsCount { get; set; }

        public int ContactFormCount { get; set; }

        public int ComplaintsCount { get; set; }

    }
}
