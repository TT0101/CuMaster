using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuMaster.Controllers.EmailAlerts
{
    public class EmailAlertRegistrationController : Controller
    {
        // GET: EmailAlertRegistration
        public ActionResult Index()
        {
            BusinessLibrary.Library.EmailAlertLibrary eal = new BusinessLibrary.Library.EmailAlertLibrary();

            return View("~/Views/EmailAlerts/EmailAlertRegistration.cshtml", eal.GetInitalSettings());
        }

        public ActionResult OnCurrencyFromChange(BusinessLibrary.ViewModels.EmailAlertViewModel emailAlert)
        {
            BusinessLibrary.Library.EmailAlertLibrary eal = new BusinessLibrary.Library.EmailAlertLibrary();

            return View("~/Views/EmailAlerts/EmailAlertRegistration.cshtml", eal.GetEmailAlert(emailAlert));
        }

        public JsonResult SaveAlert(BusinessLibrary.ViewModels.EmailAlertViewModel emailAlert)
        {
            
                BusinessLibrary.Library.EmailAlertLibrary eal = new BusinessLibrary.Library.EmailAlertLibrary();
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
            BusinessLibrary.Library.EmailAlertLibrary eal = new BusinessLibrary.Library.EmailAlertLibrary();
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