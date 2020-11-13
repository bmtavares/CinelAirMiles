namespace CinelAirMiles.Web.InternalAPI.Helpers
{
    using System.Threading.Tasks;

    using CinelAirMiles.Web.InternalAPI.Responses;

    public interface IMilesHelper
    {
        /// <summary>
        /// Calculates the miles based on the departure and arrival latitudes and longitudes.
        /// Subsuquent methods adapted to C#
        /// from https://medium.com/@RichLloydMiles/calculate-the-distance-between-two-points-on-earth-using-javascript-38e12c9a0f52
        /// and https://en.wikipedia.org/wiki/Great-circle_distance#cite_note-4
        /// </summary>
        /// <param name="depLat">Departure Latitude</param>
        /// <param name="depLong">Departure Longitude</param>
        /// <param name="arvLat">Arrival Latitude</param>
        /// <param name="arvLong">Arrival Longitude</param>
        /// <returns>Miles</returns>
        double CalculateMiles(decimal depLat, decimal depLong, decimal arvLat, decimal arvLong);
        Task<int> TicketCheckerAsync(TicketList ticketList);
    }
}
