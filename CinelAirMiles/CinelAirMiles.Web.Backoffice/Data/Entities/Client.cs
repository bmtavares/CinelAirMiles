using CinelAirMiles.Common.Entities;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinelAirMiles.Web.Backoffice.Data.Entities
{
    public class Client : IEntity
    {
        public int Id { get; set; }


        public User User { get; set; }


        public string UserId { get; set; }


        public bool Active { get; set; }


        public string MilesProgramNumber { get; set; }


        public bool IsInReferrerProgram { get; set; }


        public DateTime MembershipDate { get; set; }


        public ProgramTier ProgramTier { get; set; }


        public int ProgramTierId { get; set; }
    }
}
