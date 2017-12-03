using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.BusinessLibrary.Models
{
    public class UserAuthModel
    {
        public string UserName { get; set; }
        public bool IsValid { get; private set; }
        
        public UserAuthModel(string userName, string password)
        {
            this.UserName = userName;
            var cRes = DIResolver.Data.NinjectConfig.GetKernal().Get<CuMaster.Data.RepositoryInterfaces.IUserRepository>();
            this.IsValid = cRes.ValidateUser(this.UserName, password);
        }
    }
}
