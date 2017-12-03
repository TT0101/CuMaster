using CuMaster.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CuMaster.BusinessLibrary.Classes.Session
{
    public class SessionDefaults
    {
        public string DefaultCurrencyFrom { get; set; }
        public string DefaultCurrencyTo { get; set; }
        public string DefaultCountry { get; set; }
        public bool AutoUpdateTrackerRates { get; set; }

        public SessionDefaults()
        {

        }

        public SessionDefaults(UserEntity user, SessionDefaults locationDefaults)
        {
            if (user == null)
            {
                this.DefaultCurrencyFrom = locationDefaults.DefaultCurrencyFrom;
                this.DefaultCurrencyTo = locationDefaults.DefaultCurrencyTo;
                this.DefaultCountry = locationDefaults.DefaultCountry;
                this.AutoUpdateTrackerRates = true;
            }
            else
            {
                this.DefaultCurrencyFrom = user.DefaultCurrencyFrom ?? locationDefaults.DefaultCurrencyFrom;
                this.DefaultCurrencyTo = user.DefaultCurrencyTo ?? locationDefaults.DefaultCurrencyTo;
                this.DefaultCountry = user.DefaultCountry ?? locationDefaults.DefaultCountry;
                this.AutoUpdateTrackerRates = user.AutoUpdateEntries;
            }
        }
    }
}