using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.Security
{
    

    public class SecurityPrincipal : IPrincipal
    {
        public SecurityUser User { get; set; }
        public IIdentity Identity
        {
            get;
            private set;
        }

        public SecurityPrincipal(IIdentity identity)
        {
            Identity = identity;
        }
      

        public bool IsInRole(string role)
        {
            return true;
        }
    }
}
