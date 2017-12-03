using CuMaster.App_Start;
using CuMaster.Controllers;
using CuMaster.Security;
using Newtonsoft.Json;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace CuMaster
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private string _sessionID = null;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            DIResolver.Data.NinjectConfig.SetupNinject();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Helpers.AuthenticationHelper.CreateSessionCookie(HttpContext.Current, Helpers.AuthenticationHelper.HoursToExpire);
        }

        
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                // Get the forms authentication ticket.
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var identity = new GenericIdentity(authTicket.Name, "Forms");
                var principal = new SecurityPrincipal(identity);
                // Get the custom user data encrypted in the ticket.
                string userData = ((FormsIdentity)(Context.User.Identity)).Ticket.UserData;
                // Deserialize the json data and set it on the custom principal.
                var serializer = new JavaScriptSerializer();
                principal.User = (SecurityUser)serializer.Deserialize(userData, typeof(SecurityUser));
                // Set the context user.
                Context.User = principal;
            }
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            Helpers.AuthenticationHelper.RebuildSessionForSessionStart(HttpContext.Current);   
        }

        public void Application_Error(Object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            Server.ClearError();

            if(ex != null)
            {
                int statusCode = 500;
                if (ex.GetType() == typeof(HttpException))
                { 
                    statusCode = ((HttpException)ex).GetHttpCode();
                }

                //log and email error here; so we get full error

                //if(new HttpRequestWrapper(Context.Request).IsAjaxRequest())
                //{
                //    Context.Response.ContentType = "application/json";
                //    Context.Response.StatusCode = statusCode;
                //    Context.Response.Write(
                //        JsonConvert.SerializeObject(new { Message = ex.Message, StackTrace = ex.StackTrace })
                //        );
                //    Response.End();
                //    return;
                //}

                RouteData rd = new RouteData();
                rd.Values.Add("controller", "Error");
                rd.Values.Add("action", "Error");
                rd.Values.Add("exception", ex);
                rd.Values.Add("statuscode", statusCode);
                Session["_ERROR_"] = ex;
                Response.TrySkipIisCustomErrors = true;
                IController c = new ErrorController();
                c.Execute(new RequestContext(new HttpContextWrapper(Context), rd));
                Response.End();
            }
        }
    }
}
