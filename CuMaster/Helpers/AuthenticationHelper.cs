using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CuMaster.Security;
using System.Net.Http;
using CuMaster.BusinessLibrary.Classes.Session;
using CuMaster.BusinessLibrary.Classes;

namespace CuMaster.Helpers
{
    public static class AuthenticationHelper
    {
        private static string _phrase = "cuc35or283pp";
        public static string _userSessionKey = "CurrentUserSession";
        private static string _cookieName = "cumastercookie";
        private static double _expires = 120;

        internal static HttpCookie GetCuMasterCookie(HttpRequest request)
        {
            return CookieHelper.GetCookie(request, _cookieName);
        }

        public static void CreateSession(HttpContext context, string sessionID, Coordinates coordinates, string ipAddr)
        {
            var userCookie = CookieHelper.GetCookie(context.Request, _cookieName);
            if (userCookie == null)
            {
                CreateSessionCookie(context, _expires);
                userCookie = CookieHelper.GetCookie(context.Request, _cookieName);
            }

            BusinessLibrary.Classes.Session.Session userSession;
            if (context.Session[_userSessionKey] != null)
            {
                Session currentUserSession = (Session)context.Session[_userSessionKey];
                userSession = new Session(sessionID, userCookie.Expires, coordinates, ipAddr, (string)context.Session["LoggedInUser"], currentUserSession);
            }
            else
            {
              
                userSession = new Session(sessionID, userCookie.Expires, (string)context.Session["LoggedInUser"], coordinates, ipAddr);

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

            return sessionID;
        }

        public static string GetSessionID(HttpRequest request)
        {
            return CookieHelper.DecryptCookieValue(CookieHelper.GetCookie(request, _cookieName), _phrase);
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

        public static void RebuildSessionForUserLogIn(HttpContext context, string userID)
        {
            HttpCookie userCookie = CookieHelper.GetCookie(context.Request, _cookieName);
            Session oldSession = (Session)context.Session[_userSessionKey];
            context.Session["LoggedInUser"] = userID;
            context.Session[_userSessionKey] = new Session(oldSession.SessionID, userCookie.Expires, oldSession.Location.LocationCoordinates, oldSession.Location.IPAddress, userID, oldSession);
        }

        public static void RebuildSessionForUserLogOff(HttpContext context)
        {
            HttpCookie userCookie = CookieHelper.GetCookie(context.Request, _cookieName);
            Session oldSession = (Session)context.Session[_userSessionKey];
            context.Session["LoggedInUser"] = null;
            context.Session[_userSessionKey] = new Session(oldSession.SessionID, userCookie.Expires, oldSession.Location.LocationCoordinates, oldSession.Location.IPAddress, null, oldSession);
        }
    }
}