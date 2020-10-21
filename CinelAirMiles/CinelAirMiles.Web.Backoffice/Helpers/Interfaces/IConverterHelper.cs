using CinelAirMiles.Common.Entities;
using CinelAirMiles.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinelAirMiles.Web.Backoffice.Helpers.Interfaces
{
    public interface IConverterHelper
    {
        Mile CreateMileViewModelToMile(CreateMileViewModel model, Client client);
    }
}
