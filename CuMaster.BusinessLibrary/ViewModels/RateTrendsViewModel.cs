using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CuMaster.BusinessLibrary.ViewModels
{
    public class RateTrendsViewModel
    {
        //public IEnumerable<int> LengthsOfTimeDays { get; set; }
        public int LengthSelected { get; set; }
        public string BaseCurrency { get; set; }
        public IEnumerable<SelectListItem> Currencies { get; set; }
    }
}
