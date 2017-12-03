using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.Data.Entities
{
    public class UserEntity
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string DefaultCurrencyFrom { get; set; }
        public string DefaultCurrencyTo { get; set; }
        public string DefaultCountry { get; set; }
        public bool AutoUpdateEntries { get; set; }
    }
}
