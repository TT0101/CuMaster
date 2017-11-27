using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.Data.Entities
{
    public class BasicRateEntity
    {
        public string CurrencyFrom { get; set; }
        public string CurrencyTo { get; set; }
        public decimal Rate { get; set; }
        public DateTime TimeUpdated { get; set; }
    }
}
