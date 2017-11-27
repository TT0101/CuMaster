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

        private decimal _amountFrom;
        public decimal AmountFrom
        {
            get
            {
                return Decimal.Round(_amountFrom, 2);
            }
            set
            {
                _amountFrom = value;
            }

        }

        public string AmountFromString
        {
            get
            {
                return this.AmountFrom.ToString("0.00");
            }
        }

        private decimal _amountTo;
        public decimal AmountTo
        {
            get
            {
                return Decimal.Round(_amountTo, 2);
            }
            set
            {
                _amountTo = value;
            }

        }
        public string AmountToString
        {
            get
            {
                return this.AmountTo.ToString("0.00");
            }
        }

        public bool AutoUpdate { get; set; }
        public DateTime LastUpdated { get; set; }
        public string LastUpdatedString
        {
            get
            {
                return this.LastUpdated.ToString();
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
