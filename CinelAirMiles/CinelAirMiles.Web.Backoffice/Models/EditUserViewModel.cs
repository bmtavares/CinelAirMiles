namespace CinelAirMiles.Web.Backoffice.Models
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class EditUserViewModel : UserViewModel
    {
        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}
