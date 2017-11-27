using System;
using System.Collections.Generic;
using System.Text;

namespace CuMaster.Data.Entities
{
    public class CountryEntity : CodeNameSet
    {
        public bool UsesMetric { get; set; }
        public bool Active { get; set; }
    }
}
