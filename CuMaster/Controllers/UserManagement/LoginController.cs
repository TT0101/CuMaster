using CuMaster.BusinessLibrary.Classes.Session;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CuMaster.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View("~/Views/UserManagement/Login.cshtml");
        }

        [HttpPost]
        public JsonResult LoginUser(BusinessLibrary.UIRequestClasses.LoginRequest lr)
        {
            try
            {
                if(Security.SecurityManager.ValidateUser(lr.UserName, lr.Password, Response))
                { 
                    //need to do anything else here?
                   
                    Helpers.AuthenticationHelper.RebuildSessionForUserLogIn(System.Web.HttpContext.Current, lr.UserName);

                    return Json(new { StatusKey = "SUCCESS" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { StatusKey = "ERROR" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { StatusKey = "ERROR" }, JsonRequestBehavior.AllowGet);
            }
        }

       
        [HttpPost]
        public JsonResult LogoffUser()
        {
            Security.SecurityManager.Logoff(System.Web.HttpContext.Current.Response);
            Helpers.AuthenticationHelper.RebuildSessionForUserLogOff(System.Web.HttpContext.Current);
            
            return Json(new { StatusKey = "GOOD" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult UserIsLoggedIn()
        {
           // Session session = Helpers.AuthenticationHelper.GetSession(System.Web.HttpContext.Current);
            return Json(new { IsUserLoggedIn = User.Identity.IsAuthenticated });
        }
    }

    
}