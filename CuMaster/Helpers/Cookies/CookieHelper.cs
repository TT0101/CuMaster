using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace CuMaster.Helpers.Cookies
{
    public static class CookieHelper
    {
        public static void CreateSessionCookie(HttpResponse response, string cookieName, double expiresInHours)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Expires = DateTime.Now.AddHours(expiresInHours);
            response.Cookies.Add(cookie);

        }

        public static HttpCookie GetCookie(HttpRequest request, string cookieName)
        {
            return request.Cookies.Get(cookieName);
        }

        public static string GenerateSessionID()
        {
            Guid g = Guid.NewGuid();
            return g.ToString();
            
        }
    }
}