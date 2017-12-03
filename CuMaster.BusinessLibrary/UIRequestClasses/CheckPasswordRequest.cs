using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.BusinessLibrary.UIRequestClasses
{
    public class CheckPasswordRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
