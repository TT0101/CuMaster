using System;
using System.Collections.Generic;
using System.Text;

namespace CuMaster.BusinessLibrary.Models
{
    public class CurrencyRateModel
    {
        public CurrencyModel FromCurrency { get; set; }
        public CurrencyModel ToCurrency { get; set; }
        public decimal Rate { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        public CurrencyRateModel(Data.Entities.CurrencyRateEntity currencyRate)
        {
            this.FromCurrency = new Lookups.CurrencyLookup().GetItemFromLookup(currencyRate.FromCurrency);
            this.ToCurrency = new Lookups.CurrencyLookup().GetItemFromLookup(currencyRate.ToCurrency);
            this.Rate = currencyRate.Rate;
            this.LastUpdatedOn = currencyRate.LastUpdated;
        }

        /// <summary>
        /// Sets model up as a 1-to-1 currency rate for the currency provided
        /// </summary>
        /// <param name="currencyFrom"></param>
        public CurrencyRateModel(string currencyFrom)
        {
            this.FromCurrency = new Lookups.CurrencyLookup().GetItemFromLookup(currencyFrom);
            this.ToCurrency = new Lookups.CurrencyLookup().GetItemFromLookup(currencyFrom);
            this.Rate = 1;
            this.LastUpdatedOn = DateTime.Now.ToUniversalTime();
        }

        public decimal Convert(decimal value)
        {
            return value * this.Rate;
        }
    }
}
