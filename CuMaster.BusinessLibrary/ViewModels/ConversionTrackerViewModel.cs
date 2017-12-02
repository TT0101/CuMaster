using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.BusinessLibrary.ViewModels
{
    public class ConversionTrackerViewModel
    {
        public int EntryID { get; set; }
        public string EntryName { get; set; }
        public string CurrencyFrom { get; set; }
        public string CurrencyTo { get; set; }
        public decimal RateUsed { get; set; }
        public bool IsCrypto { get; set; }

        private decimal _amountFrom;
        public decimal AmountFrom
        {
            get
            {
                if (this.CurrencyTo != null && this.IsCrypto)
                {
                    return Decimal.Round(_amountFrom, 9);
                }
                else
                {
                    return Decimal.Round(_amountFrom, 2);
                }
            }
            set
            {
                _amountFrom = value;
            }

        }

        private decimal _amountTo;
        public decimal AmountTo
        {
            get
            {
                if (this.CurrencyTo != null && this.IsCrypto)
                {
                    return Decimal.Round(_amountTo, 9);
                }
                else
                {
                    return Decimal.Round(_amountTo, 2);
                }
            }
            set
            {
                _amountTo = value;
            }

        }

        public bool AutoUpdate { get; set; }
        public DateTime LastUpdated { get; set; }
        public string LastUpdatedString
        {
            get
            {
                return this.LastUpdated.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            }
            set
            {
                DateTime newDate;
                if(DateTime.TryParse(value, out newDate))
                {
                    this.LastUpdated = newDate;
                }

            }
        }
    }
}
