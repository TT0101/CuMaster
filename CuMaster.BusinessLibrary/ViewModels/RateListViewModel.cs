using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.BusinessLibrary.ViewModels
{
    public class RateListViewModel
    {
        public string BaseCurrency { get; set; }

        public string CurrencyTo { get; set; }
        public string CurrencyToName { get; set; }
        public decimal RateFowards { get; set; }

        public decimal RateInverse { get; set; }

        public DateTime DateUpdated { get; set; }
        public string DateUpdatedUTCStr
        {
            get
            {
                return this.DateUpdated.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            }
        }
        public bool IsFromRateCrypto { get; set; }
        public bool IsToRateCrypto { get; set; }
    }
}
