namespace CinelAirMiles.Web.Backoffice.Helpers.Classes
{
    using CinelAirMiles.Common.Entities;
    using CinelAirMiles.Web.Backoffice.Helpers.Interfaces;
    using CinelAirMiles.Web.Backoffice.Models;

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
    }
}
