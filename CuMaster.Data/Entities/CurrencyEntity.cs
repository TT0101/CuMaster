using System;
using System.Collections.Generic;
using System.Text;

namespace CuMaster.Data.Entities
{
    public class CurrencyEntity
    {
        public string ID { get; set; }
        public string Name { get; set; }
        //public List<string> Countries { get; set; }
        public bool IsCryptocurrency { get; set; }
        public string ASCIISymbol { get; set; }
        public string HTMLSymbol { get; set; }
        public bool Active { get; set; }


    }
}
