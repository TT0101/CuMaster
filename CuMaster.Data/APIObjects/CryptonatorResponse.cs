using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.Data.APIObjects
{
    public class CryptonatorResponse
    {
        public CryptonatorTickerObject Ticker { get; set; }
        public long timestamp { get; set; }
        public bool success { get; set; }
        public string error { get; set; }
    }
}
