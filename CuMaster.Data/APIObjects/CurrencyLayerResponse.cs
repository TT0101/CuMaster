using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.Data.APIObjects
{
    public class CurrencyLayerResponse
    {
        public bool success { get; set; }
        public CurrencyLayerError error { get; set; }
        public string source { get; set; }
        public IDictionary<string, string> quotes { get; set; }
        public long timestamp { get; set; }
    }
}
