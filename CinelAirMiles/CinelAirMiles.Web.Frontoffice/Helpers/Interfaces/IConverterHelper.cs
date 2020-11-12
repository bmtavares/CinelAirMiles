using CinelAirMiles.Common.Entities;
using CinelAirMiles.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CinelAirMiles.Web.Frontoffice.Helpers.Interfaces
{
    public interface IConverterHelper
    {
        List<MilesViewModel> FromMileToMilesViewModel(IEnumerable<Mile> miles);
    }
}
