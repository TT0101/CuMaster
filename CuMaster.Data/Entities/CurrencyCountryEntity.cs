using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.Data.Entities
{
    public class CurrencyCountryEntity
    {
        public string CurrencyCd { get; set; }
        public string CountryCd { get; set; }
        public bool Active { get; set; }
    }
}
