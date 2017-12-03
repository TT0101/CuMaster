using CuMaster.BusinessLibrary.Classes.Session;
using CuMaster.BusinessLibrary.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CuMaster.Controllers.UserManagement
{
    public class UserDashboardController : Controller
    {
        private string UserID { get; set; }

        // GET: UserDashboard
        public ActionResult Index()
        {
            Session session = Helpers.AuthenticationHelper.GetSession(System.Web.HttpContext.Current);
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "AccessDenied", new { area = "" });
            }
            else
            {
                if(session.UserID == "" || session.UserID == null)
                {
                    Helpers.AuthenticationHelper.RebuildSessionForUserLogIn(System.Web.HttpContext.Current, User.Identity.Name, true);
                }
            }

            BusinessLibrary.Library.UserSelfAdministrationLibrary uLib = new BusinessLibrary.Library.UserSelfAdministrationLibrary(System.Web.HttpContext.Current, session);
            return View("~/Views/UserManagement/UserDashboard.cshtml", uLib.GetUserDashboardInformation());
        }

        [HttpPost]
        public JsonResult OnCurrencyChanged(BusinessLibrary.ViewModels.UserDashboardViewModel ud)
        {
            Session session = Helpers.AuthenticationHelper.GetSession(System.Web.HttpContext.Current);
            BusinessLibrary.Library.UserSelfAdministrationLibrary uLib = new BusinessLibrary.Library.UserSelfAdministrationLibrary(System.Web.HttpContext.Current, session);
            return Json(uLib.GetUserDashboardFromViewModel(ud), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CheckEmail(string Email)
        {
            Session session = Helpers.AuthenticationHelper.GetSession(System.Web.HttpContext.Current);
            UserRegistrationLibrary lib = new UserRegistrationLibrary();
            BusinessLibrary.Library.UserSelfAdministrationLibrary uLib = new BusinessLibrary.Library.UserSelfAdministrationLibrary(System.Web.HttpContext.Current, session);
            if(Email == uLib.GetCurrentSavedEmail())
                return Json(new { Valid = true }, JsonRequestBehavior.AllowGet);
            else if (!lib.IsEmailTaken(Email))
                return Json(new { Valid = true }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Valid = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveProfile(BusinessLibrary.ViewModels.UserDashboardViewModel ud)
        {
            try
            {
                Session session = Helpers.AuthenticationHelper.GetSession(System.Web.HttpContext.Current);
                BusinessLibrary.Library.UserSelfAdministrationLibrary uLib = new BusinessLibrary.Library.UserSelfAdministrationLibrary(System.Web.HttpContext.Current, session);
                uLib.SaveUserProfile(ud);
                return Json(new { StatusKey = "SUCCESS" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { StatusKey = "ERROR" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult SaveDefaults(BusinessLibrary.ViewModels.UserDashboardViewModel ud)
        {
            try
            {
                Session session = Helpers.AuthenticationHelper.GetSession(System.Web.HttpContext.Current);
                BusinessLibrary.Library.UserSelfAdministrationLibrary uLib = new BusinessLibrary.Library.UserSelfAdministrationLibrary(System.Web.HttpContext.Current, session);
                uLib.SaveUserDefaults(ud);
                Helpers.AuthenticationHelper.RebuildSessionForUserLogIn(System.Web.HttpContext.Current, session.UserID);
                return Json(new { StatusKey = "SUCCESS" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { StatusKey = "ERROR" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}