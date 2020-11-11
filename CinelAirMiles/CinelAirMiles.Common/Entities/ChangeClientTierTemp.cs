namespace CinelAirMiles.Common.Entities
{
    public class ChangeClientTierTemp : IEntity
    {
        public int Id { get; set; }


        public Client Client { get; set; }


        public int ClientId { get; set; }


        public ProgramTier ProgramTier { get; set; }


        public int ProgramTierId { get; set; }
    }
}
