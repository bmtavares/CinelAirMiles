namespace CinelAirMiles.Common.Entities
{
    using System.ComponentModel.DataAnnotations;
    
    public class ProgramTier : IEntity
    {
        public int Id { get; set; }


        [Display(Name = "Tier description")]
        public string Description { get; set; }


        public double MilesMultiplier { get; set; }
    }
}
