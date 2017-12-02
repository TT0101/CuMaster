using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.BusinessLibrary.UIRequestClasses
{
    public class UserLocationRequest
    {
        public Classes.Coordinates Coords { get; set; }
        public string IP { get; set; }
    }
}
