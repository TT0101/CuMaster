using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.BusinessLibrary.Library
{
    public class UserRegistrationLibrary
    {
        public UserRegistrationLibrary()
        {

        }

        public bool IsUserNameTaken(string userName)
        {
            var res = DIResolver.Data.NinjectConfig.GetKernal().Get<CuMaster.Data.RepositoryInterfaces.IUserRepository>();
            return res.CheckUserName(userName);
        }

        public bool IsEmailTaken(string email)
        {
            var res = DIResolver.Data.NinjectConfig.GetKernal().Get<CuMaster.Data.RepositoryInterfaces.IUserRepository>();
            return res.CheckEmail(email);
        }

        public bool DoesPasswordMeetRequirements(string password, string userName)
        {
            return Security.PasswordRules.MeetsLengthRule(password) && Security.PasswordRules.MeetsEntropyRule(password) && Security.PasswordRules.MeetsNotSameAsUsernameRule(password, userName);
        }

        public void RegisterUser(BusinessLibrary.Models.RegisterModel user, string sessionID, DateTime dateExpires)
        {
            var res = DIResolver.Data.NinjectConfig.GetKernal().Get<CuMaster.Data.RepositoryInterfaces.IUserRepository>();
            res.CreateAccount(user.UserName, user.Password, user.Email, user.DisplayName, sessionID, dateExpires); 
        }
    }
}
