namespace CinelAirMiles.Web.Frontoffice.Helpers.Interfaces
{
    using System.Collections.Generic;
    using CinelAirMiles.Common.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetCreditCards(IEnumerable<CreditCardInfo> creditCards);
    }
}
