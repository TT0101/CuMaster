using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CuMaster.BusinessLibrary.Lookups;

namespace CuMaster.BusinessLibrary.Models
{
    public class CurrencyModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string HTMLSymbol { get; set; }
        public bool IsCryptocurrency { get; set; }
        public IEnumerable<CountryModel> Countries { get; set; }

        public CurrencyModel(Data.Entities.CurrencyEntity currency)
        {
            this.ID = currency.ID;
            this.Name = currency.Name;
            this.IsCryptocurrency = currency.IsCryptocurrency;
            this.Countries = new CurrencyCountryLookup().GetForFirstKey(currency.ID);
            this.HTMLSymbol = currency.HTMLSymbol;
        }

        public bool IsForCountry(string countryCd)
        {
            return this.Countries.Any(c => c.Code == countryCd);
        }
    }
}
