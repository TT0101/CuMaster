using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.UpdateCurrencyRates
{
    public class APICurrencyRates
    {
        public string CurrencyCdFrom { get; set; }
        public string CurrencyCdTo { get; set; }
        public string Delimiter { get; set; }
        public string CurrencyPairID { get; private set; }
        public decimal Rate { get; set; }
        public DateTime TimeUpdated { get; set; }

        public APICurrencyRates(string currencyFrom, string currencyTo, string delimiter)
        {
            this.CurrencyCdFrom = currencyFrom;
            this.CurrencyCdTo = currencyTo;
            this.CurrencyPairID = currencyFrom + delimiter + currencyTo;
            this.Rate = -1;
            this.TimeUpdated = DateTime.Now.ToUniversalTime();
        }
    }
}
