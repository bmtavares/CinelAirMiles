namespace CinelAirMiles.Web.InternalAPI.Helpers
{
    public interface IMathHelper
    {
        double ConvertDegreesToRadians(double angle);

        double ConvertRadiansToDegrees(double angle);

        double CalculateGreatCircleDistance(double angle);

        /// <summary>
        /// Calculates the Central Subtended Angle.
        /// For use in this project, assume X and Y are Departure and Arrival.
        /// </summary>
        /// <param name="xLat">X Latitude</param>
        /// <param name="xLong">X Longitude</param>
        /// <param name="yLat">Y Latitude</param>
        /// <param name="yLong">Y Longitude</param>
        /// <returns>Central Subtended Angle</returns>
        double CalculateCentralSubtendedAngle(decimal xLat, decimal xLong, decimal yLat, decimal yLong);
    }
}
