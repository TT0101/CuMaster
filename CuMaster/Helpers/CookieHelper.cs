using CuMaster.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CuMaster.Helpers
{
    public static class CookieHelper
    {
        public static HttpCookie GetCookie(HttpRequest request, string cookieName)
        {
            return request.Cookies.Get(cookieName);
        }

        public static string DecryptCookieValue(HttpCookie cookie, string phrase)
        {
            if (cookie == null)
                return "";

            return StringEncryption.Decrypt(cookie.Value, phrase);
        }

        public static HttpCookie CreateCookie(string value, string cookieName, string phrase, double hoursToExpire)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Value = StringEncryption.Encrypt(value, phrase);
            cookie.Expires = DateTime.Now.AddHours(hoursToExpire);

            return cookie;
        }
    }
}