using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.Data.Entities
{
    public class ConversionTrackerEntity
    {
        public int EntryID { get; set; }
        public string EntryName { get; set; }
        public string CurrencyFromCd { get; set; }
        public string CurrencyToCd { get; set; }
        public string CurrencyFrom { get; set; }
        public string CurrencyTo { get; set; }
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; set; }
        public decimal RateToUse { get; set; }
        public bool UpdateRate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string SessionID { get; set; }
        public bool IsCrypto { get; set; }
    }
}
