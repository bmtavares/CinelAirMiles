namespace CinelAirMiles.Web.Backoffice.Helpers.Interfaces
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetProgramTiers();
        IEnumerable<SelectListItem> GetComboMilesTypes();
    }
}
