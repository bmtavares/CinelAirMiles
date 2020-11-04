namespace CinelAirMiles.Web.Backoffice.Helpers.Classes
{
    using System.Collections.Generic;
    using System.Linq;

    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Web.Backoffice.Helpers.Interfaces;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CombosHelper : ICombosHelper
    {
        readonly ApplicationDbContext _context;

        public CombosHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetProgramTiers()
        {
            var list = _context.ProgramTiers.Select(
                mt => new SelectListItem
                {
                    Text = mt.Description,
                    Value = mt.Id.ToString()
                }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a program tier...",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboMilesTypes()
        {
            var list = _context.MilesTypes.Select(
                mt => new SelectListItem
                {
                    Text = mt.Description,
                    Value = mt.Id.ToString()
                }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a mile type...",
                Value = "0"
            });

            return list;
        }
    }
}
