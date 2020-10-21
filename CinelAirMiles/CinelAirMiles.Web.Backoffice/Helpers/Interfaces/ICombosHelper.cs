using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinelAirMiles.Web.Backoffice.Helpers.Interfaces
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboMilesTypes();
    }
}
