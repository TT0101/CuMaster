using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CuMaster.BusinessLibrary.Classes
{
    public class Session
    {
        public string SessionID { get; set; }
        public DateTime DateExpired { get; set; }
        public SessionDefaults Defaults { get; set; }
        public SessionLocation Location { get; set; }

        public Session()
        {

        }

        public Session(Models.UserModel user)
        {

        }
    }
}