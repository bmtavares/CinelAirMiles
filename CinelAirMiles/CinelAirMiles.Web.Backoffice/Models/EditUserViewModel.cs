namespace CinelAirMiles.Web.Backoffice.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class EditUserViewModel : UserViewModel
    {
        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}
