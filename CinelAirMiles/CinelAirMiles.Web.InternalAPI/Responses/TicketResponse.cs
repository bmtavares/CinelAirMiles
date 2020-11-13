namespace CinelAirMiles.Web.InternalAPI.Responses
{
    using Newtonsoft.Json;

    public class TicketResponse
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MilesProgramNumber { get; set; }

        [JsonProperty("DLat")]
        public decimal DepartureLatitude { get; set; }

        [JsonProperty("DLong")]
        public decimal DepartureLongitude { get; set; }

        [JsonProperty("ALat")]
        public decimal ArrivalLatitude { get; set; }
        
        [JsonProperty("ALong")]
        public decimal ArrivalLongitude { get; set; }

        [JsonProperty("DReg")]
        public int DepartureRegion { get; set; }

        [JsonProperty("AReg")]
        public int ArrivalRegion { get; set; }

        public int SeatClassId { get; set; }
    }
}
