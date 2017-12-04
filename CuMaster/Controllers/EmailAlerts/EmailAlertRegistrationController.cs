using CuMaster.BusinessLibrary.Classes.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuMaster.Controllers.EmailAlerts
{
    public class EmailAlertRegistrationController : Controller
    {
        private Session session { get; set; }
        // GET: EmailAlertRegistration
        public ActionResult Index()
        {
            Session session = Helpers.AuthenticationHelper.GetSession(System.Web.HttpContext.Current);

            BusinessLibrary.Library.EmailAlertLibrary eal = new BusinessLibrary.Library.EmailAlertLibrary(System.Web.HttpContext.Current, session);

            return View("~/Views/EmailAlerts/EmailAlertRegistration.cshtml", eal.GetInitalSettings());
        }
    
        public ActionResult OnCurrencyFromChange(BusinessLibrary.ViewModels.EmailAlertViewModel emailAlert)
        {
            Session session = Helpers.AuthenticationHelper.GetSession(System.Web.HttpContext.Current);
            BusinessLibrary.Library.EmailAlertLibrary eal = new BusinessLibrary.Library.EmailAlertLibrary(System.Web.HttpContext.Current, session);

            return View("~/Views/EmailAlerts/EmailAlertRegistration.cshtml", eal.GetEmailAlert(emailAlert));
        }

        public JsonResult SaveAlert(BusinessLibrary.ViewModels.EmailAlertViewModel emailAlert)
        {
            Session session = Helpers.AuthenticationHelper.GetSession(System.Web.HttpContext.Current);
            BusinessLibrary.Library.EmailAlertLibrary eal = new BusinessLibrary.Library.EmailAlertLibrary(System.Web.HttpContext.Current, session);
                try
                {
                    eal.SaveEmailAlert(emailAlert);
                    return Json(new { StatusKey = "SUCCESS" });
                }
                catch
                {
                    return Json(new { StatusKey = "ERROR" });
                }
         
        }

        public JsonResult DeleteAlerts(string email)
        {
            Session session = Helpers.AuthenticationHelper.GetSession(System.Web.HttpContext.Current);
            BusinessLibrary.Library.EmailAlertLibrary eal = new BusinessLibrary.Library.EmailAlertLibrary(System.Web.HttpContext.Current, session);
            try
            {
                eal.DeleteAllAlerts(email);
                return Json(new { StatusKey = "SUCCESS" });
            }
            catch
            {
                return Json(new { StatusKey = "ERROR" });
            }
        }
    }
}