using CuMaster.BusinessLibrary.Classes.Session;
using HelperFramework.UI.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuMaster.Controllers.UserManagement
{
    public class ManageEmailAlertsController : Controller
    {
        // GET: ManageEmailAlerts
        public ActionResult Index()
        {
            Session session = Helpers.AuthenticationHelper.GetSession(System.Web.HttpContext.Current);
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "AccessDenied", new { area = "" });
            }
            else
            {
                if (session.UserID == "" || session.UserID == null)
                {
                    Helpers.AuthenticationHelper.RebuildSessionForUserLogIn(System.Web.HttpContext.Current, User.Identity.Name, true);
                }
            }

            return View("~/Views/UserManagement/ManageEmailAlerts.cshtml");
        }

        [HttpPost]
        public JsonResult GetUserEmailAlerts(DataTableParams dtp)
        {
            if (User.Identity.IsAuthenticated)
            {
                string userName = User.Identity.Name;
                BusinessLibrary.Library.ManageEmailAlertLibrary mLib = new BusinessLibrary.Library.ManageEmailAlertLibrary();
                return Json(mLib.GetUserEmailAlerts(userName, dtp).ListDataTable);
            }

            return Json(new DataTableObject<string>());
        }

        [HttpPost]
        public JsonResult DeleteAlert(BusinessLibrary.UIRequestClasses.TrackerUpdateRequest alertRequest)
        {
            try
            {

                BusinessLibrary.Library.ManageEmailAlertLibrary mLib = new BusinessLibrary.Library.ManageEmailAlertLibrary();
                mLib.DeleteEmailAlert(alertRequest.entryID);
                return Json(new { StatusKey = "SUCCESS" });
            }
            catch
            {
                return Json(new { StatusKey = "ERROR" });
            }
        }

        [HttpPost]
        public JsonResult DeleteAllAlerts()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    string userName = User.Identity.Name;
                    BusinessLibrary.Library.ManageEmailAlertLibrary mLib = new BusinessLibrary.Library.ManageEmailAlertLibrary();
                    mLib.DeleteAllUserAlerts(userName);
                    return Json(new { StatusKey = "SUCCESS" });
                }
                return Json(new { StatusKey = "ERROR" });
            }
            catch
            {
                return Json(new { StatusKey = "ERROR" });
            }
        }
    }
}