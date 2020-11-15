namespace CinelAirMiles.Common.Entities
{
    public class SeatClass : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double RegularMultiplier { get; set; }

        public double InternationalMultiplier { get; set; }
    }
}
