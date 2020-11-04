namespace CinelAirMiles.Web.Backoffice.Models
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class EditClientViewModel : UserViewModel
    {
        public int ClientId { get; set; }

        public int FlownSegments { get; set; }

        public DateTime MembershipDate { get; set; }

        public int ProgramTierId { get; set; }

        public IEnumerable<SelectListItem> ProgramTiers { get; set; }
    }
}
