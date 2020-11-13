namespace CinelAirMiles.Web.Frontoffice.Helpers.Classes
{
    using System.Collections.Generic;
    using System.Linq;
    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;
    using CinelAirMiles.Web.Frontoffice.Helpers.Interfaces;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CombosHelper : ICombosHelper
    {
        readonly ApplicationDbContext _context;

        public CombosHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetCreditCards(IEnumerable<CreditCardInfo> creditCards)
        {
            var list = creditCards.Select(
                cc => new SelectListItem
                {
                    Text = $"**** **** {cc.Number.Substring(8)}",
                    Value = cc.Id.ToString()
                }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Use existing card...",
                Value = "0"
            });

            return list;
        }
    }
}
