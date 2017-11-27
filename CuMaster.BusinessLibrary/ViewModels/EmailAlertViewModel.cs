using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CuMaster.BusinessLibrary.ViewModels
{
    public class EmailAlertViewModel
    {
        [Required()]
        [EmailAddress]
        public string Email { get; set; }

        [Required()]
        [StringLength(10)]
        public string CurrencyFrom { get; set; }

        [Required()]
        [StringLength(10)]
        public string CurrencyTo { get; set; }

        [RegularExpression("([0-9]+[.,]{1}[0-9]+)", ErrorMessage = "Please enter a valid number")]
        public decimal PercentageChange { get; set; }
        
        [DataType(DataType.Time)]
        public TimeSpan TimeToSend { get; set; }

        public IEnumerable<SelectListItem> Currencies { get; set; }
        public IEnumerable<SelectListItem> CurrenciesTo { get; set; }

        public EmailAlertViewModel()
        {
            this.Currencies = new List<SelectListItem>();
            this.CurrenciesTo = new List<SelectListItem>();
        }

    }
}
