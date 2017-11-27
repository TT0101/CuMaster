using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.BusinessLibrary.Models
{
    public class ConversionTrackerEntryModel
    {
        public int EntryID { get; set; }
        public string EntryName { get; set; }
        public int RateID { get; set; }
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; set; }
        public decimal RateToUse { get; set; }
        public bool UpdateRate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}
