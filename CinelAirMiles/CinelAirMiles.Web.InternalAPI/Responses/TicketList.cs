namespace CinelAirMiles.Web.InternalAPI.Responses
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class TicketList
    {
        [JsonProperty("results")]
        public List<TicketResponse> tickets { get; set; }
    }
}
