namespace CinelAirMiles.Web.Backoffice.Helpers.Classes
{
    using CinelAirMiles.Common.Entities;
    using CinelAirMiles.Common.Models;
    using CinelAirMiles.Web.Backoffice.Helpers.Interfaces;
    using System;

    public class ConverterHelper : IConverterHelper
    {
        public Mile CreateMileViewModelToMile(CreateMileViewModel model, Client client)
        {
            return new Mile
            {
                Miles = model.Miles,
                Client = client,
                MilesType = model.MilesType,
                CreditDate = model.CreditDate,
                ExpiryDate = model.ExpiryDate
            };
        }
    }
}
