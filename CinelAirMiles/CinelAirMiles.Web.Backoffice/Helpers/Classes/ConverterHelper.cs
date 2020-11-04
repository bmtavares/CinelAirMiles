namespace CinelAirMiles.Web.Backoffice.Helpers.Classes
{
    using CinelAirMiles.Common.Entities;
    using CinelAirMiles.Web.Backoffice.Helpers.Interfaces;
    using CinelAirMiles.Web.Backoffice.Models;
    using System.Collections.Generic;

    public class ConverterHelper : IConverterHelper
    {
        public Mile CreateMileViewModelToMile(CreateMileViewModel model, Client client, MilesType mileType)
        {
            return new Mile
            {
                Miles = model.Miles,
                Client = client,
                MilesTypeId = mileType.Id,
                CreditDate = model.CreditDate,
                ExpiryDate = model.ExpiryDate,
                Description = model.Description
            };
        }

        public List<UserViewModel> UsersToUserViewModels(List<User> users)
        {
            var models = new List<UserViewModel>();
            
            foreach(var user in users)
            {
                models.Add(new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                });
            }

            return models;
        }

        public EditUserViewModel UserToEditUserViewModel(User user)
        {
            return new EditUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
        }

        public UserViewModel UserToUserViewModel(User user)
        {
            return new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public EditClientViewModel ClientToEditClientViewModel(User user, Client client)
        {
            return new EditClientViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                //Email = user.Email,
                ClientId = client.Id,
                FlownSegments = client.FlownSegments,
                MembershipDate = client.MembershipDate,
                ProgramTierId = client.ProgramTierId
            };
        }
    }
}
