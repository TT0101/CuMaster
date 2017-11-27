using CuMaster.BusinessLibrary.Models;
using CuMaster.BusinessLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace CuMaster.ViewModels
{
    public class CurrencyConversionViewModel
    {
        public decimal ValueFrom { get; set; }
        public decimal ValueTo { get; set; }
        public CurrencyViewModel CurrencyFrom { get; set; }
        public CurrencyViewModel CurrencyTo { get; set; }
        public decimal FromRate { get; set; }
        public IList<SelectListItem> Currencies { get; set; }
        public DateTime CurrencyFromLastUpdated { get; set; }

       
    }
}
