using CinelAirMiles.Common.Entities;
using CinelAirMiles.Common.Models;
using CinelAirMiles.Web.Backoffice.Models;

namespace CinelAirMiles.Web.Backoffice.Helpers.Interfaces
{
    public interface IConverterHelper
    {
        Mile CreateMileViewModelToMile(CreateMileViewModel model, Client client, MilesType mileType);
    }
}
