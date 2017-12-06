using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.BusinessLibrary.UIRequestClasses
{
    public class RateChartDataRequest
    {
        public string CurrencyFrom { get; set; }
        public string CurrencyTo { get; set; }
        public int Days { get; set; }
    }
}
