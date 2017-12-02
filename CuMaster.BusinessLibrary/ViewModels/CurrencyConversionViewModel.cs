using CuMaster.BusinessLibrary.Models;
using CuMaster.BusinessLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace CuMaster.BusinessLibrary.ViewModels
{
    public class CurrencyConversionViewModel
    {
        private decimal _valueFrom;
        [Required()] //decimal validation?? 19,9
        public decimal ValueFrom
        {
            get
            {
                if (this.CurrencyTo != null && this.CurrencyTo.IsCrypto)
                {
                    return Decimal.Round(_valueFrom, 9);
                }
                else
                {
                    return Decimal.Round(_valueFrom, 2);
                }
            }
            set
            {
                _valueFrom = value;
            }

        }

        private decimal _valueTo;
        [Required()]
        public decimal ValueTo
        {
            get
            {
                if (this.CurrencyTo != null && this.CurrencyTo.IsCrypto)
                {
                    return Decimal.Round(_valueTo, 9);
                }
                else
                {
                    return Decimal.Round(_valueTo, 2);
                }
            }
            set
            {
                _valueTo = value;
            }

        }

        [Required()]
        public CurrencyViewModel CurrencyFrom { get; set; }

        [Required()] //dd check?
        public CurrencyViewModel CurrencyTo { get; set; }


        private decimal _fromRate;
        public decimal FromRate
        {
            get
            {
                if (this.CurrencyTo != null && this.CurrencyTo.IsCrypto)
                {
                    return Decimal.Round(_fromRate, 9);
                }
                else
                {
                    return Decimal.Round(_fromRate, 2);
                }
            }
            set
            {
                _fromRate = value;
            }
        }

        public decimal FromRateFull
        {
            get
            {
                return _fromRate;
            }
        }
        public List<SelectListItem> Currencies { get; set; }
        public IEnumerable<SelectListItem> CurrenciesTo { get; set; }
        public DateTime CurrencyFromLastUpdated { get; set; }
        public string CurrencyFromLastUpdatedUTCString
        {
            get
            {
                return this.CurrencyFromLastUpdated.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"); 
            }
        }

    }
}
