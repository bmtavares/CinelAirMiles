using System.ComponentModel.DataAnnotations;

namespace CinelAirMiles.Common.Entities
{
    public class ProgramTier : IEntity
    {
        public int Id { get; set; }


        [Display(Name = "Tier description")]
        public string Description { get; set; }
    }
}
