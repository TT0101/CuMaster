using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CuMaster.BusinessLibrary.Classes.Session
{
    public class Session
    {
        public string SessionID { get; private set; }
        public string UserID { get; private set; }
        public DateTime DateExpired { get; private set; }
        public SessionDefaults Defaults { get; set; }
        public SessionLocation Location { get; set; }

        public Session(string sessionID, DateTime expires, string userLoggedIn, Coordinates coords, string ipAddr)
        {
            this.SessionID = sessionID;
            this.DateExpired = expires;
            this.Location = new SessionLocation(coords, ipAddr);
            if (userLoggedIn == "")
            {
                this.Defaults = GetSessionDefaultsFromLocation();
            }
            else
            {
                //get from db for user
                this.Defaults = GetSessionDefaultsForUser();
            }

            this.UserID = userLoggedIn;
        }

        public Session(string sessionID, DateTime expires, Coordinates newCoords, string newIP, string userLoggedIn, Session oldSession)
        {
            this.SessionID = oldSession.SessionID;
            this.DateExpired = expires;

            if (oldSession.Location.LocationCoordinates.IsFarAway(newCoords))
            {
                this.Location = new SessionLocation(newCoords, newIP);
                if(userLoggedIn == null)
                {
                    this.Defaults = GetSessionDefaultsFromLocation();
                }
                else
                {
                    this.Defaults = GetSessionDefaultsForUser();
                }
            }
            else
            {
                this.Location = oldSession.Location;
                this.Location.IPAddress = newIP;
            }
        }

        public bool IsUserLoggedIn()
        {
            return (this.UserID != "") ? true : false;
        }

        private SessionDefaults GetSessionDefaultsFromLocation()
        {
            return new SessionDefaults(); //will possibly implement this, but this is something that's not going to be noticible in use...
        }

        private SessionDefaults GetSessionDefaultsForUser()
        {
            return new SessionDefaults(); //actually call from db later when we have user repo
        }

        //private string GetCurrencyFromLocation()
        //{
        //    //call geocoding with credentials

        //    //get currency
        //}

    }
}