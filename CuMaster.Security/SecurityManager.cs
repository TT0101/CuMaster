using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.Security
{
    public class SecurityManager
    {
        public SecurityUser User { get; private set; }

        public SecurityManager()
        {
            this.User = new SecurityUser();
        }

    }
}
