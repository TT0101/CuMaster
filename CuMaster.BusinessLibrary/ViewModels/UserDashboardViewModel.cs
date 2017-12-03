using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CuMaster.BusinessLibrary.ViewModels
{
    public class UserDashboardViewModel
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }

        public string DefaultCurrencyFrom { get; set; }
        public string DefaultCurrencyTo { get; set; }
        public string DefaultCountry { get; set; }
        public bool AutoUpdateTrackerDefault { get; set; }

        public IEnumerable<SelectListItem> Currencies { get; set; }
        public IEnumerable<SelectListItem> CurrenciesTo { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }


    }
}
