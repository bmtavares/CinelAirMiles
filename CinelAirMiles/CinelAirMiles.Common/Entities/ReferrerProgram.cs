namespace CinelAirMiles.Common.Entities
{
    public class ReferrerProgram
    {
        public Client ReferrerClient { get; set; }


        public int ReferrerClientId { get; set; }


        public Client ReferredClient { get; set; }


        public int ReferredClientId { get; set; }
    }
}
