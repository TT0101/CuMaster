using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CuMaster.BusinessLibrary.Classes.Session
{
    public class SessionLocation
    {
        public string Country { get; set; }
        public Coordinates LocationCoordinates { get; set; }
        public string IPAddress { get; set; }

        public SessionLocation(Coordinates userCoordinates, string ipAddr)
        {
            this.LocationCoordinates = userCoordinates;
            this.IPAddress = ipAddr;
        }

        public void SetCountry()
        {
            if(this.LocationCoordinates.Latitude > 0)
            {
                Data.APIHandlers.GoogleGeocodingAPIHandler handler = new Data.APIHandlers.GoogleGeocodingAPIHandler();

            }
            else if(this.IPAddress != null && this.IPAddress != "0")
            {

            }
        }
    }
}