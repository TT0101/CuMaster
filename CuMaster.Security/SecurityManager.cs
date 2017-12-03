using HelperFramework.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace CuMaster.Security
{
    //a lot of this logic via https://github.com/primaryobjects/MVC4FormsAuthentication. Database grab code is my own.
    public class SecurityManager
    {
        

        public SecurityManager()
        {
            
        }

        public static SecurityUser User
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return ((SecurityPrincipal)(HttpContext.Current.User)).User;
                }
                else if (HttpContext.Current.Items.Contains("User"))
                {
                    // The user is not authenticated, but has successfully logged in.
                    return (SecurityUser)HttpContext.Current.Items["User"];
                }
                else
                {
                    return null;
                }
            }
        }

        public static bool ValidateUser(string username, string password, HttpResponseBase response)
        {
            bool result = false;
            if (Membership.ValidateUser(username, password))
            {

                var serializer = new JavaScriptSerializer();
                string userData = serializer.Serialize(SecurityManager.User);
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                        username,
                        DateTime.Now,
                        DateTime.Now.AddDays(30),
                        true,
                        userData,
                        FormsAuthentication.FormsCookiePath);
                // Encrypt the ticket.
                string encTicket = FormsAuthentication.Encrypt(ticket);
                // Create the cookie.
                response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                result = true;
            }
            return result;
        }

        public static SecurityUser AuthenticateUser(string username, string password)
        {
            SecurityUser user = null;
            using (var context = new DataAccessFramework.DBConnection.DBConnectionContext(DatabaseName.CuMaster))
            {
                SqlParameter[] sparams = new SqlParameter[1];
                sparams[0] = new SqlParameter("UserName", username);

                //have to do this because ie is an ass about getting this for some reason...
                int counter = 0;
                int limit = 10;
                UserAuthEntity u = null;
                do
                {
                   u = context.ExecuteSproc<UserAuthEntity>("usp_GetUserCredentals", sparams).FirstOrDefault();
                   counter++;
                } while (u == null || counter > limit);

                if (u != null && Security.PasswordHash.ValidatePassword(password, u.Hash))
                {
                   SqlParameter[] sparams2 = new SqlParameter[1];
                   sparams2[0] = new SqlParameter("UserName", username);

                   return context.ExecuteSproc<SecurityUser>("usp_GetUser", sparams2).FirstOrDefault();
                }
            }

            return user;
        }

        public static void Logoff(HttpResponse response)
        {
           
            // Delete the authentication ticket and sign out.
            FormsAuthentication.SignOut();
            // Clear authentication cookie.
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            response.Cookies.Add(cookie);
        }

    }
}
