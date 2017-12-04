using CuMaster.BusinessLibrary.Classes.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuMaster.Controllers.EmailAlerts
{
    public class EmailAlertEditController : Controller
    {
        // GET: EmailAlertEdit
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoadForAlertEdit(int alertID)
        {
            Session session = Helpers.AuthenticationHelper.GetSession(System.Web.HttpContext.Current);

            BusinessLibrary.Library.EmailAlertLibrary eal = new BusinessLibrary.Library.EmailAlertLibrary(System.Web.HttpContext.Current, session);
            return PartialView("~/Views/UserManagement/EmailAlertEdit.cshtml", eal.GetForExistingAlert(alertID));
        }

    }
}