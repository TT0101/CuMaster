using CuMaster.BusinessLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using CuMaster.BusinessLibrary.Models;
using System.Web.Mvc;
using CuMaster.Data.Entities;

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
        //public IEnumerable<CurrencyModel> Currencies { get; set; }
        //private List<SelectListItem> CurrencySelect { get; set; }

        //private SelectListGroup cryptoGroup = new SelectListGroup { Name = "Cryptocurrency" };
        //private SelectListGroup countryGroup = new SelectListGroup { Name = "Country-Based Currency" };

        public CurrencyConversionLibrary()
        {
            this.DefaultFromCurrency = "USD";
            this.DefaultToCurrency = "ISK";
            this.ValueFrom = 1;

            this.Currencies = new CurrencyLibrary();

            this.ChangeCurrencyRates(this.DefaultFromCurrency, this.DefaultToCurrency);
            
        }

        public CurrencyConversionLibrary(string currencyFrom, string currencyTo) :this()
        {
            this.ChangeCurrencyRates(currencyFrom, currencyTo);
        }

        public CurrencyConversionLibrary(CurrencyConversionViewModel ccModel, bool updateReversed) : this()
        {
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
