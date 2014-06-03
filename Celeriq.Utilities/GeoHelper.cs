using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celeriq.Utilities
{
    /// <summary />
    public class GeoHelper
    {
        /// <summary />
        public static bool InRadius(double? lat1, double? long1, double? lat2, double? long2, double radius)
        {
            var d = Calc(lat1, long1, lat2, long2);
            if (d == null) return false;
            return (d.Value <= radius);
        }

        /// <summary />
        public static bool OutRadius(double? lat1, double? long1, double? lat2, double? long2, double radius)
        {
            var d = Calc(lat1, long1, lat2, long2);
            if (d == null) return false;
            return (d.Value > radius);
        }

        /// <summary />
        public static double? Calc(double? lat1, double? long1, double? lat2, double? long2)
        {
            if (lat1 == null || long1 == null ||
                lat2 == null || long2 == null)
                return null;

            #region Documentation

            /*
					The Haversine formula according to Dr. Math.
					http://mathforum.org/library/drmath/view/51879.html
								
					dlon = lon2 - lon1
					dlat = lat2 - lat1
					a = (sin(dlat/2))^2 + cos(lat1) * cos(lat2) * (sin(dlon/2))^2
					c = 2 * atan2(sqrt(a), sqrt(1-a)) 
					d = R * c
								
					Where
							* dlon is the change in longitude
							* dlat is the change in latitude
							* c is the great circle distance in Radians.
							* R is the radius of a spherical Earth.
							* The locations of the two points in 
									spherical coordinates (longitude and 
									latitude) are lon1,lat1 and lon2, lat2.
			*/

            #endregion

            var dDistance = Double.MinValue;
            var dLat1InRad = lat1.Value*(Math.PI/180.0);
            var dLong1InRad = long1.Value*(Math.PI/180.0);
            var dLat2InRad = lat2.Value*(Math.PI/180.0);
            var dLong2InRad = long2.Value*(Math.PI/180.0);

            var dLongitude = dLong2InRad - dLong1InRad;
            var dLatitude = dLat2InRad - dLat1InRad;

            // Intermediate result a.

            var a = Math.Pow(Math.Sin(dLatitude/2.0), 2.0) +
                    Math.Cos(dLat1InRad)*Math.Cos(dLat2InRad)*
                    Math.Pow(Math.Sin(dLongitude/2.0), 2.0);

            // Intermediate result c (great circle distance in Radians).

            var c = 2.0*Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));

            // Distance.

            // const Double kEarthRadiusMiles = 3956.0;

            const Double kEarthRadiusKms = 6376.5;
            dDistance = kEarthRadiusKms*c;

            return dDistance;
        }
    }
}