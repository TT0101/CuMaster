using CuMaster.BusinessLibrary.Models;
using CuMaster.Data.Entities;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CuMaster.BusinessLibrary.Library
{
    public class CurrencyLibrary
    {
        public IEnumerable<CurrencyModel> CurrenciesFrom { get; set; }
        public IEnumerable<SelectListItem> CurrencyFromSelect { get; set; }

        private SelectListGroup cryptoGroup = new SelectListGroup { Name = "Cryptocurrency" };
        private SelectListGroup countryGroup = new SelectListGroup { Name = "Country-Based Currency" };


        public CurrencyLibrary()
        {
            this.GetUseableCurrencies();
        }

        public void GetUseableCurrencies()
        {
            var cRes = DIResolver.Data.NinjectConfig.GetKernal().Get<CuMaster.Data.RepositoryInterfaces.ICurrencyRepository>();
            this.CurrenciesFrom = cRes.GetActiveCurrenciesWithCurrentRates().Select(c => new CurrencyModel(c));
            this.CurrencyFromSelect = this.CurrenciesFrom.Select(c => new SelectListItem { Value = c.ID, Text = c.Name, Group = (c.IsCryptocurrency) ? cryptoGroup : countryGroup }).ToList();
        }

        public IEnumerable<CurrencyModel> GetAllowedCurrencies(string selectedCurrency)
        {
            //turns out, getting it from the db is much faster in this case
            var acRes = DIResolver.Data.NinjectConfig.GetKernal().Get<CuMaster.Data.RepositoryInterfaces.IAllowedCurrencyRepository>();
            return acRes.GetFor(selectedCurrency).Select(ce => new CurrencyModel(ce));
        }

        public IEnumerable<SelectListItem> GetAllowedCurrenciesSelect(string selectedCurrency)
        {
            return this.GetAllowedCurrencies(selectedCurrency).Select(c => new SelectListItem { Value = c.ID, Text = c.Name, Group = (c.IsCryptocurrency) ? cryptoGroup : countryGroup }).ToList();
        }

        public Tuple<Models.CurrencyRateModel, Models.CurrencyRateModel> GetFromToRatePair(string currencyFrom, string currencyTo, string defaultFrom, string defaultTo)
        {
            var crRes = DIResolver.Data.NinjectConfig.GetKernal().Get<CuMaster.Data.RepositoryInterfaces.ICurrencyRateRepository>();
            Tuple<Models.CurrencyRateModel, Models.CurrencyRateModel> set;

            if (currencyFrom != currencyTo)
            {
                CurrencyRateEntity rateFromTo = crRes.GetSingle(currencyFrom, currencyTo);
                //CurrencyRateEntity rateToFrom = crRes.GetSingle(currencyTo, currencyFrom);

                //if you can't find the selected one, get the the defaults.  If you can't get the defaults, get the first ones....
                if (rateFromTo == null)
                {
                    rateFromTo = crRes.GetRatesForCurrency(currencyFrom, true).FirstOrDefault(); //get the first pair using the currency from provided

                    if (rateFromTo == null && currencyFrom != defaultFrom) //if that doesn't work, try finding the to currency provided paired with the default
                    {
                        rateFromTo = crRes.GetSingle(defaultFrom, currencyTo);
                    }
                    if (rateFromTo == null)
                    {
                        rateFromTo = crRes.Get().FirstOrDefault(); //if not, just get the first one
                    }
                }

                CurrencyRateEntity rateToFrom = new CurrencyRateEntity
                {
                    FromCurrency = rateFromTo.ToCurrency,
                    ToCurrency = rateFromTo.FromCurrency,
                    Rate = (rateFromTo.Rate == 0) ? 0 : (1 / rateFromTo.Rate),
                    LastUpdated = rateFromTo.LastUpdated
                };

                //if (rateToFrom == null)
                //{
                //    rateToFrom = crRes.GetSingle(currencyTo, rateFromTo.FromCurrency); //try to get the opposite pair with the current to currency and whatever was found above
                //    if (rateToFrom == null && currencyTo != defaultTo) //if this doesn't work, try the default currency with what was selected above
                //    {
                //        rateToFrom = crRes.GetSingle(defaultTo, rateFromTo.FromCurrency);
                //    }
                //    if (rateToFrom == null) //if that doesn't work, get the first one you can find
                //    {
                //        rateToFrom = crRes.GetRatesForCurrency(rateFromTo.ToCurrency).FirstOrDefault();
                //    }
                //}

                set = new Tuple<CurrencyRateModel, CurrencyRateModel>(this.CreateRateModel(rateFromTo), this.CreateRateModel(rateToFrom));

            }
            else
            {
                var eRate = this.CreateEqualRateModel(currencyFrom);
                set = new Tuple<CurrencyRateModel, CurrencyRateModel>(eRate, eRate);
            }

            return set;
        }

        private CurrencyRateModel CreateRateModel(Data.Entities.CurrencyRateEntity crEntity)
        {
            if (crEntity == null)
                return null;

            return new CurrencyRateModel(crEntity);
        }

        private CurrencyRateModel CreateEqualRateModel(string currencyFrom)
        {
            var cLook = new Lookups.CurrencyLookup(true);

            return new CurrencyRateModel(currencyFrom);
        }
    }
}
