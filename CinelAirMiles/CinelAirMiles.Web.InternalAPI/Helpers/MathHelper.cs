namespace CinelAirMiles.Web.InternalAPI.Helpers
{
    using System;

    public class MathHelper : IMathHelper
    {
        public double ConvertDegreesToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }

        public double ConvertRadiansToDegrees(double angle)
        {
            return (180 / Math.PI) * angle;
        }

        public double CalculateGreatCircleDistance(double angle)
        {
            const int EarthRadius = 6371;

            return 2 * Math.PI * EarthRadius * (angle / 360);
        }

        public double CalculateCentralSubtendedAngle(decimal xLat, decimal xLong, decimal yLat, decimal yLong)
        {
            var xA = Convert.ToDouble(xLat);
            var xB = Convert.ToDouble(xLong);
            var yA = Convert.ToDouble(yLat);
            var yB = Convert.ToDouble(yLong);

            xA = ConvertDegreesToRadians(xA);
            xB = ConvertDegreesToRadians(xB);
            yA = ConvertDegreesToRadians(yA);
            yB = ConvertDegreesToRadians(yB);

            //var sinLatitudeX = Math.Sin(xA);
            //var sinLatitudeY = Math.Sin(yA);

            var cosLatitudeX = Math.Cos(xA);
            var cosLatitudeY = Math.Cos(yA);

            var latitudeDelta = Math.Abs(xA - yA);
            var longitudeDelta = Math.Abs(xB - yB);

            //var vicentyUpper = Math.Sqrt(
            //                            Math.Pow(
            //                                cosLatitudeY * Math.Sin(longitudeDelta)
            //                            , 2)
            //                            + 
            //                            Math.Pow(
            //                                cosLatitudeX * sinLatitudeY - sinLatitudeX * cosLatitudeY 
            //                                * Math.Cos(longitudeDelta)
            //                            , 2)
            //                        );

            //var vicentyLower = sinLatitudeX * sinLatitudeY + cosLatitudeX * cosLatitudeY *
            //                            Math.Cos(longitudeDelta);


            //var vicenty = Math.Atan(vicentyUpper/vicentyLower);


            var haversine = 2 * Math.Asin(
                                        Math.Sqrt( Math.Pow( Math.Sin( latitudeDelta / 2 ), 2) 
                                                + cosLatitudeX * cosLatitudeY *
                                                Math.Pow( Math.Sin( longitudeDelta / 2 ), 2)
                                            )
                                    );

            return ConvertRadiansToDegrees(haversine);
        }
    }
}
