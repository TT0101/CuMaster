using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.BusinessLibrary.Classes
{
    public class Coordinates
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public Coordinates()
        {

        }

        public void SetCoordinates(decimal lat, decimal longi)
        {
            this.Latitude = lat;
            this.Longitude = longi;
        }

        internal bool IsFarAway(Coordinates newCoords)
        {
            if (this.DistanceAwayFromCurrentLocation(newCoords, true) > 10000)
            {
                return true;
            }

            return false;
        }

        public double DistanceAwayFromCurrentLocation(Coordinates coords, bool returnMiles)
        {
            int radiusMi = 3959;
            int radiusKm = 6373;

            double dlong = (double)coords.Longitude - (double)this.Longitude;
            double dlat = (double)coords.Latitude - (double)this.Latitude;
            double a = Math.Pow((Math.Sin(dlat / 2)), 2) + Math.Cos((double)coords.Latitude) * Math.Cos((double)this.Latitude) * Math.Pow((Math.Sin(dlong / 2)), 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return (returnMiles) ? radiusMi : radiusKm * c;
        }
    }
}
