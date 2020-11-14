namespace CinelAirMiles.Web.Backoffice.Helpers.Interfaces
{
    using System.Collections.Generic;

    using CinelAirMiles.Common.Entities;
    using CinelAirMiles.Web.Backoffice.Models;

    public interface IConverterHelper
    {
        Mile CreateMileViewModelToMile(CreateMileViewModel model, Client client, MilesType mileType);

        UserViewModel UserToUserViewModel(User user);

        EditUserViewModel UserToEditUserViewModel(User user);

        List<UserViewModel> UsersToUserViewModels(List<User> users);

        EditClientViewModel ClientToEditClientViewModel(User user, Client client);
        
    }
}
