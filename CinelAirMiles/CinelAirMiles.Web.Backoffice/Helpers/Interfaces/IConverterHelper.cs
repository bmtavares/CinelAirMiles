using CinelAirMiles.Common.Entities;
using CinelAirMiles.Common.Models;
using CinelAirMiles.Web.Backoffice.Models;
using System.Collections.Generic;

namespace CinelAirMiles.Web.Backoffice.Helpers.Interfaces
{
    public interface IConverterHelper
    {
        Mile CreateMileViewModelToMile(CreateMileViewModel model, Client client, MilesType mileType);

        UserViewModel UserToUserViewModel(User user);

        EditUserViewModel UserToEditUserViewModel(User user);

        List<UserViewModel> UsersToUserViewModels(List<User> users);
    }
}
