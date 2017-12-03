using CuMaster.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuMaster.Data.RepositoryInterfaces
{
    public interface IUserRepository
    {
        UserEntity GetUser(string userName);
        bool ValidateUser(string username, string password);
        void CreateAccount(string username, string password, string email, string displayName, string sessionID, DateTime expires);
        bool CheckUserName(string username);
        bool CheckEmail(string email);

        void UpdateUserCookie(string username, string cookieID, DateTime dateExpires);
        string GetUserIDByCookieID(string cookieID);

        void UpdateProfile(Entities.UserEntity userPref);
        void UpdateDefaults(Entities.UserEntity userPref);

    }
}
