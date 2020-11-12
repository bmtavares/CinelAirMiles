using CinelAirMiles.Common.Entities;
using CinelAirMiles.Common.Models;
using CinelAirMiles.Web.Frontoffice.Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinelAirMiles.Web.Frontoffice.Helpers.Classes
{
    public class ConverterHelper : IConverterHelper
    {
        public List<MilesViewModel> FromMileToMilesViewModel(IEnumerable<Mile> miles)
        {
            var model = new List<MilesViewModel>();

            foreach (var mile in miles) {
                model.Add(new MilesViewModel
                {
                    Miles = mile.Miles,
                    CreditDate = mile.CreditDate,
                    ExpiryDate = mile.ExpiryDate,
                    Description = mile.Description,
                    Type = mile.MilesType.Description
                });
            }

            return model;
        }
    }
}
