namespace CinelAirMiles.Web.Backoffice.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using CinelAirMiles.Common.Models;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CreateUserViewModel : RegisterNewUserViewModel
    {
        //TOOO: Make this prop required
        //[Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
    }

}
