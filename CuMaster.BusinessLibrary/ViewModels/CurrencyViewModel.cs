using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuMaster.BusinessLibrary.Models;

namespace CuMaster.BusinessLibrary.ViewModels
{
    public class CurrencyViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string HTMLSymbol { get; set; }

        public CurrencyViewModel()
        {

        }

        public CurrencyViewModel(Models.CurrencyModel cModel)
        {
            this.ID = cModel.ID;
            this.Name = cModel.Name;
            this.HTMLSymbol = cModel.HTMLSymbol;
        }
    }
}
