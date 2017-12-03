using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CuMaster.Security;
using System.Net.Http;
using CuMaster.BusinessLibrary.Classes.Session;
using CuMaster.BusinessLibrary.Classes;
using Ninject;

namespace CuMaster.Helpers
{
    public static class AuthenticationHelper
    {
        private static string _phrase = "cuc35or283pp";
        public static string _userSessionKey = "CurrentUserSession";
        private static string _cookieName = "cumastercookie";
        public static double HoursToExpire = 500;

        internal static Session GetSession(HttpContext context)
        {
            return (Session)context.Session[_userSessionKey];
        }

        internal static HttpCookie GetCuMasterCookie(HttpRequest request)
        {
            return CookieHelper.GetCookie(request, _cookieName);
        }

        public static void CreateSession(HttpContext context, string sessionID, Coordinates coordinates, string ipAddr)
        {
            var userCookie = CookieHelper.GetCookie(context.Request, _cookieName);
            if (userCookie == null)
            {
                CreateSessionCookie(context, HoursToExpire);
                userCookie = CookieHelper.GetCookie(context.Request, _cookieName);
            }
            string userID = (context.User.Identity.IsAuthenticated) ? context.User.Identity.Name : "";
            BusinessLibrary.Classes.Session.Session userSession;
            if (context.Session[_userSessionKey] != null)
            {
                Session currentUserSession = (Session)context.Session[_userSessionKey];
                userSession = new Session(sessionID, userCookie.Expires, coordinates, ipAddr, userID, currentUserSession);
            }
            else
            {
              
                userSession = new Session(sessionID, userCookie.Expires, userID, coordinates, ipAddr);

            }

            context.Session[_userSessionKey] = userSession;

        }

        public static string CreateSessionCookie(HttpContext context, double expiresInHours)
        {
            HttpCookie cookie;
            string sessionID = "";
            if (context.Response.Cookies.AllKeys.Contains(_cookieName))
            {
                cookie = CookieHelper.GetCookie(context.Request, _cookieName);
                sessionID = CookieHelper.DecryptCookieValue(cookie, _phrase);
                context.Response.Cookies.Remove(_cookieName);
            }
            else
            {
                sessionID = GenerateSessionID();
            }

            cookie = CookieHelper.CreateCookie(sessionID, _cookieName, _phrase, expiresInHours);

            context.Response.Cookies.Add(cookie);

            if (GetSession(context) != null)
            {
                string userID = GetSession(context).UserID;
                if (context.User.Identity.IsAuthenticated && (userID != "" || userID != null))
                {
                    UpdateUserCookie(context, sessionID, userID);
                }
            }
            
            return sessionID;
        }

        public static void UpdateUserCookie(HttpContext context, string sessionID, string userName)
        {
            if(userName != "" || SecurityManager.User.UserID != "" || userName != null || SecurityManager.User.UserID != null)
            {
                string userID = userName ?? SecurityManager.User.UserID;
                var res = DIResolver.Data.NinjectConfig.GetKernal().Get<CuMaster.Data.RepositoryInterfaces.IUserRepository>();
                res.UpdateUserCookie(userID, sessionID, CookieHelper.GetCookie(context.Request, _cookieName).Expires);
            }
        }

        public static string GetSessionID(HttpContext context)
        {
            string sessionID = CookieHelper.DecryptCookieValue(CookieHelper.GetCookie(context.Request, _cookieName), _phrase);
            if(sessionID == "" || sessionID == null)
            {
                CreateSessionCookie(context, HoursToExpire);
                sessionID = CookieHelper.DecryptCookieValue(CookieHelper.GetCookie(context.Request, _cookieName), _phrase);
            }

            return sessionID;
        }

        public static string GenerateSessionID()
        {
            Guid g = Guid.NewGuid();
            return g.ToString();
        }

        public static void ClearSession(HttpContext context)
        {
            context.Session[_userSessionKey] = null;
        }

        public static void RebuildSessionForUserLogIn(HttpContext context, string userName, bool recreate = false)
        {
            HttpCookie userCookie = CookieHelper.GetCookie(context.Request, _cookieName);
            Session oldSession = (Session)context.Session[_userSessionKey];
            string userID = userName;
            context.Session[_userSessionKey] = new Session(oldSession.SessionID, userCookie.Expires, oldSession.Location.LocationCoordinates, oldSession.Location.IPAddress, userID, oldSession);
            if (!recreate)
            {
                UpdateUserCookie(context, oldSession.SessionID, userName); //this is more to move records, the cookie should be the same as before
            }
        }

        internal static void RebuildSessionForSessionStart(HttpContext context)
        {
            //HttpCookie userCookie = CookieHelper.GetCookie(context.Request, _cookieName);
            //if(userCookie == null)
            //{

            //}
            Helpers.AuthenticationHelper.CreateSessionCookie(context, Helpers.AuthenticationHelper.HoursToExpire);
            HttpCookie userCookie = CookieHelper.GetCookie(context.Request, _cookieName);

            string sessionID = CookieHelper.DecryptCookieValue(userCookie, _phrase);
            if (context.User.Identity.IsAuthenticated)
            {
                string userID = context.User.Identity.Name;
                context.Session[_userSessionKey] = new Session(sessionID, userCookie.Expires, SecurityManager.User, new Coordinates(), "0");
            }
            else
            {
                context.Session[_userSessionKey] = new Session(sessionID, userCookie.Expires, "", new Coordinates(), "0");
            }
        }

        public static void RebuildSessionForUserLogOff(HttpContext context)
        {
            HttpCookie userCookie = CookieHelper.GetCookie(context.Request, _cookieName);
            Session oldSession = (Session)context.Session[_userSessionKey];
            if (oldSession != null)
            {
                context.Session[_userSessionKey] = new Session(oldSession.SessionID, userCookie.Expires, oldSession.Location.LocationCoordinates, oldSession.Location.IPAddress, "", oldSession);
            }
            else
            {
                string sessionID = GetSessionID(context);
                CreateSession(context, sessionID, new Coordinates(), "0");
            }
        }

        public static void RebuildSessionForLocationFound(HttpContext context, Coordinates coords, string ipAddr)
        {
            HttpCookie userCookie = CookieHelper.GetCookie(context.Request, _cookieName);
            if(userCookie == null)
            {
                CreateSessionCookie(context, HoursToExpire);
            }
            userCookie = CookieHelper.GetCookie(context.Request, _cookieName);
            Session oldSession = (Session)context.Session[_userSessionKey];
            string userID = (context.User.Identity.IsAuthenticated) ? context.User.Identity.Name : "";
            if (oldSession != null)
            {
                context.Session[_userSessionKey] = new Session(oldSession.SessionID, userCookie.Expires, coords, ipAddr, userID, oldSession);
            }
            else
            {
                string sessionID = GetSessionID(context);
                CreateSession(context, sessionID, coords, ipAddr);
            }
        }
    }
}