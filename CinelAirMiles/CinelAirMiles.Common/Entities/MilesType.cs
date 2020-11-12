using System.ComponentModel.DataAnnotations;

namespace CinelAirMiles.Common.Entities
{
    public class MilesType : IEntity
    {
        public int Id { get; set; }


        [Display(Name = "Type")]
        public string Description { get; set; }
    }
}
