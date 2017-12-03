using CuMaster.BusinessLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using CuMaster.BusinessLibrary.Models;
using System.Web.Mvc;
using CuMaster.Data.Entities;
using CuMaster.BusinessLibrary.Classes.Session;

namespace CuMaster.BusinessLibrary.Library
{
    public class CurrencyConversionLibrary
    {
        private string DefaultFromCurrency { get; set; }
        private string DefaultToCurrency { get; set; }
        public CurrencyRateModel RateFromTo { get; set; }
        public CurrencyRateModel RateToFrom { get; set; }
        public DateTime LastUpdated { get; set; }
        public decimal ValueFrom { get; set; }
        public decimal ValueTo { get; set; }

        public CurrencyLibrary Currencies { get; set; }


        public CurrencyConversionLibrary(Classes.Session.Session currentSession)
        {
            this.SetDefaults(currentSession);
            this.ValueFrom = 1;

            this.Currencies = new CurrencyLibrary();

            this.ChangeCurrencyRates(this.DefaultFromCurrency, this.DefaultToCurrency);
            
        }

        private void SetDefaults(Classes.Session.Session currentSession)
        {
            if (currentSession != null)
            {
                SessionDefaults defaults = currentSession.GetSessionDefaultsForUser(currentSession.UserID);
                this.DefaultFromCurrency = defaults.DefaultCurrencyFrom;
                this.DefaultToCurrency = defaults.DefaultCurrencyTo;
            }
            else
            {
                this.DefaultToCurrency = "EUR";
                this.DefaultFromCurrency = "USD";
            }
        }

        public CurrencyConversionLibrary(Session currentSession, string currencyFrom, string currencyTo)
        {
            this.SetDefaults(currentSession);
            this.ValueFrom = 1;

            this.Currencies = new CurrencyLibrary();
            this.ChangeCurrencyRates(currencyFrom, currencyTo);
        }

        public CurrencyConversionLibrary(Session currentSession, CurrencyConversionViewModel ccModel, bool updateReversed)
        {
            this.SetDefaults(currentSession);

            this.Currencies = new CurrencyLibrary();
            this.ValueFrom = ccModel.ValueFrom;
            this.ValueTo = ccModel.ValueTo;
            this.ChangeCurrencyRates(ccModel.CurrencyFrom.ID, ccModel.CurrencyTo.ID, updateReversed);
        }

        public void ChangeCurrencyRates(string currencyFrom, string currencyTo, bool updateReversed = false)
        {
            Tuple<Models.CurrencyRateModel, Models.CurrencyRateModel> ratePair = this.Currencies.GetFromToRatePair(currencyFrom, currencyTo, this.DefaultFromCurrency, this.DefaultToCurrency);
            this.RateFromTo = ratePair.Item1;
            this.RateToFrom = ratePair.Item2;

            this.UpdateRate(updateReversed);
        }


        public void UpdateRate(bool updateFrom)
        {
            if (updateFrom)
            {
                if (this.ValueTo != 0)
                    this.ValueFrom = this.RateToFrom.Convert(this.ValueTo);
                else
                    this.ValueFrom = 0;
            }
            else
            {
                this.ValueTo = this.RateFromTo.Convert(this.ValueFrom);
            }
        }

        public CurrencyConversionViewModel GetCurrencyConversion()
        {

            return new CurrencyConversionViewModel()
            {
                Currencies = this.Currencies.CurrencyFromSelect.ToList()
                , CurrencyFrom = new CurrencyViewModel(this.RateFromTo.FromCurrency)
                , CurrencyTo = new CurrencyViewModel(this.RateFromTo.ToCurrency)
                , ValueFrom = this.ValueFrom
                , ValueTo = this.ValueTo
                , FromRate = this.RateFromTo.Rate
                , CurrencyFromLastUpdated = this.RateFromTo.LastUpdatedOn
                , CurrenciesTo = this.Currencies.GetAllowedCurrenciesSelect(this.RateFromTo.FromCurrency.ID)
            };
        }

       

        //private UserCurrencyDefaults GetUserDefaults()
        //{

        //}
    }
}
