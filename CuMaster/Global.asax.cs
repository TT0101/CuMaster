using CuMaster.App_Start;
using CuMaster.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace CuMaster
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            DIResolver.Data.NinjectConfig.SetupNinject();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        
        protected void Application_AuthenticationRequest(object sender, EventArgs e)
        {
            Security.SecurityManager sm = new Security.SecurityManager();
            try
            {
                HttpCookie userCookie = Helpers.Cookies.CookieHelper.GetCookie(Request, "cumanagercookie");
                if(userCookie == null)
                {
                    //create cookie
                    Helpers.Cookies.CookieHelper.CreateSessionCookie(Response, "cumanagercookie", 1);
                }

                if (userCookie == null || string.IsNullOrEmpty(userCookie.Value))
                {
                    //delete cookie
                }

                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(userCookie.Value);

                if(authTicket == null)
                {
                    //delete cookie
                }

                //create GenericIdentity, user data
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Application_Error(Object ender, EventArgs e)
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

                if(new HttpRequestWrapper(Context.Request).IsAjaxRequest())
                {
                    Context.Response.ContentType = "application/json";
                    Context.Response.StatusCode = statusCode;
                    Context.Response.Write(
                        JsonConvert.SerializeObject(new { Message = ex.Message, StackTrace = ex.StackTrace })
                        );
                    Response.End();
                    return;
                }

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
