using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.BusinessLibrary.Models
{
    public class EmailAlertModel
    {
        public string Email { get; set; }

        public string CurrencyFrom { get; set; }

        public string CurrencyTo { get; set; }

        public decimal? PercentageChange { get; set; }

        public TimeSpan? TimeToSend { get; set; }
    }
}
