using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.Data.APIObjects
{
    public class CryptonatorTickerObject
    {
        [JsonProperty("base")]
        public string baseCurrency { get; set; }
        public string target { get; set; }
        public decimal price { get; set; }
       
    }
}
