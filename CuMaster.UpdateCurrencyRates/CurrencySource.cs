using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.UpdateCurrencyRates
{
    internal class CurrencySource
    {
        public string CurrencyCd { get; set; }
        public string SourceFrom { get; set; }
        public string SourceTo { get; set; }
        public bool Active { get; set; }

    }
}
