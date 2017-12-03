using CuMaster.Data.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuMaster.Data.Entities;
using System.Data.SqlClient;
using HelperFramework.Configuration;

namespace CuMaster.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        public bool CheckEmail(string email)
        {
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                SqlParameter[] sparams = new SqlParameter[1];
                sparams[0] = new SqlParameter("Email", email);

                int result = context.ExecuteSproc<int>("usp_CheckEmail", sparams).FirstOrDefault();
                return (result == 1) ? true : false;
            }
        }

        public bool CheckUserName(string username)
        {
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                SqlParameter[] sparams = new SqlParameter[1];
                sparams[0] = new SqlParameter("UserName", username);

                int result = context.ExecuteSproc<int>("usp_CheckUserName", sparams).FirstOrDefault();
                return (result == 1) ? true : false;
            }
        }

        public void CreateAccount(string username, string password, string email, string displayName, string sessionID, DateTime dateExpires)
        {
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                SqlParameter[] sparams = new SqlParameter[6];
                sparams[0] = new SqlParameter("UserName", username);
                sparams[1] = new SqlParameter("Password", password);
                sparams[2] = new SqlParameter("Email", email);
                sparams[3] = new SqlParameter("DisplayName", displayName);
                sparams[4] = new SqlParameter("SessionID", sessionID);
                sparams[5] = new SqlParameter("DateExpires", dateExpires);

                context.ExecuteNonResultSproc("usp_SaveNewUser", sparams);
            }
        }

        public UserEntity GetUser(string userName)
        {
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                SqlParameter[] sparams = new SqlParameter[1];
                sparams[0] = new SqlParameter("UserName", userName);

                return context.ExecuteSproc<UserEntity>("usp_GetUser", sparams).FirstOrDefault();
            }
        }

        public string GetUserIDByCookieID(string cookieID)
        {
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                SqlParameter[] sparams = new SqlParameter[1];
                sparams[0] = new SqlParameter("SessionID", cookieID);

                return context.ExecuteSproc<string>("usp_GetUserIDByCookie", sparams).FirstOrDefault();
            }
        }

        public void UpdateDefaults(UserEntity userPref)
        {
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                SqlParameter[] sparams = new SqlParameter[5];
                sparams[0] = new SqlParameter("UserName", userPref.UserName);
                sparams[1] = new SqlParameter("CurrencyFrom", (userPref.DefaultCurrencyFrom) ?? "");
                sparams[2] = new SqlParameter("CurrencyTo", (userPref.DefaultCurrencyTo) ?? "");
                sparams[3] = new SqlParameter("Country", (userPref.DefaultCountry) ?? "");
                sparams[4] = new SqlParameter("AutoUpdateEntries", userPref.AutoUpdateEntries);

                context.ExecuteNonResultSproc("usp_SaveUserDefaults", sparams);
            }
        }

        public void UpdateProfile(UserEntity userPref)
        {
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                SqlParameter[] sparams = new SqlParameter[3];
                sparams[0] = new SqlParameter("UserName", userPref.UserName);
                sparams[1] = new SqlParameter("Email", userPref.Email);
                sparams[2] = new SqlParameter("DisplayName", userPref.DisplayName);

                context.ExecuteNonResultSproc("usp_SaveUserProfile", sparams);
            }
        }

        public void UpdateUserCookie(string username, string cookieID, DateTime dateExpires)
        {
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                SqlParameter[] sparams = new SqlParameter[3];
                sparams[0] = new SqlParameter("UserID", username);
                sparams[1] = new SqlParameter("SessionID", cookieID);
                sparams[2] = new SqlParameter("DateExpires", (dateExpires.Year == 1) ? DateTime.Now.AddDays(1) : dateExpires);

                context.ExecuteNonResultSproc("usp_UpdateUserCookie", sparams);
            }
        }

        public bool ValidateUser(string username, string password)
        {
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                SqlParameter[] sparams = new SqlParameter[1];
                sparams[0] = new SqlParameter("UserName", username);

                UserAuthEntity u = context.ExecuteSproc<UserAuthEntity>("usp_GetUserCredentals", sparams).FirstOrDefault();

                if (Security.PasswordHash.ValidatePassword(password, u.Hash))
                    return true;
            }

            return false;
        }
    }
}
