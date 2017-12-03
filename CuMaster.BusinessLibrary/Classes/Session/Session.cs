using CuMaster.Data.Entities;
using CuMaster.Security;
using Ninject;
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
            this.DateExpired = (expires.Year == 1) ? DateTime.Now.AddDays(1) : expires;
            this.Location = new SessionLocation(coords, ipAddr);
            this.UserID = userLoggedIn;

            if (userLoggedIn == "" || userLoggedIn == null)
            {
                this.Defaults = GetSessionDefaultsFromLocation();
            }
            else
            {
                //get from db for user
                this.Defaults = GetSessionDefaultsForUser(userLoggedIn);
                    
            }

        }

        public Session(string sessionID, DateTime expires, SecurityUser user, Coordinates coords, string ipAddr)
        {
            this.SessionID = sessionID;
            this.DateExpired = (expires.Year == 1) ? DateTime.Now.AddDays(1) : expires;
            this.Location = new SessionLocation(coords, ipAddr);
            this.UserID = user.UserID;
            if (user != null && user.UserID != "")
            {
                this.Defaults = GetSessionDefaultsForUser(user.UserID);
            }
            else
            {
                this.Defaults = GetSessionDefaultsFromLocation();
            }
        }

        public Session(string sessionID, DateTime expires, Coordinates newCoords, string newIP, string userLoggedIn, Session oldSession)
        {
            this.SessionID = oldSession.SessionID;
            this.UserID = userLoggedIn;
            this.DateExpired = (expires.Year == 1) ? DateTime.Now.AddDays(1) : expires;
            if(newCoords == null)
            {
                newCoords = new Coordinates();
            }
            
            if ((oldSession.Location.LocationCoordinates.Latitude == 0 && oldSession.Location.IPAddress == null) || oldSession.Location.LocationCoordinates.IsFarAway(newCoords))
            {
                this.Location = new SessionLocation(newCoords, newIP);
            }
            else
            {
                this.Location = oldSession.Location;
                this.Location.IPAddress = newIP;
            }

            if (userLoggedIn == null || userLoggedIn == "")
            {
                this.Defaults = GetSessionDefaultsFromLocation();
            }
            else
            {
                this.Defaults = GetSessionDefaultsForUser(this.UserID);
            }
        }

        //public bool IsUserLoggedIn()
        //{
        //    return (this.UserID != "" && this.UserID != null) ? true : false;
        //}

        private SessionDefaults GetSessionDefaultsFromLocation()
        {
            return new SessionDefaults
            {
                DefaultCountry = "USA",
                DefaultCurrencyFrom = "USD",
                DefaultCurrencyTo = "EUR",
                AutoUpdateTrackerRates = true
                
            }; //will possibly implement this, but this is something that's not going to be noticible in use...
        }

        public SessionDefaults GetSessionDefaultsForUser(string userName)
        {
            if(userName == null)
            {
                return this.GetSessionDefaultsFromLocation();
            }

            //return new SessionDefaults(); //actually call from db later when we have user repo
            var res = DIResolver.Data.NinjectConfig.GetKernal().Get<CuMaster.Data.RepositoryInterfaces.IUserRepository>();
            UserEntity user = res.GetUser(userName);
            return new SessionDefaults(user, this.GetSessionDefaultsFromLocation());
        }

        public void RefreshForDefaultChange(string userName)
        {
            this.Defaults = this.GetSessionDefaultsForUser(userName);
        }

        //private string GetCurrencyFromLocation()
        //{
        //    //call geocoding with credentials

        //    //get currency
        //}

    }
}