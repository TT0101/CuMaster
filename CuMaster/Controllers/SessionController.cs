using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuMaster.Controllers
{
    public class SessionController : Controller
    {
        // GET: Session
        public ActionResult Index()
        {
            return View("~/Views/Shared/Session.cshtml");
        }

        /// <summary>
        /// this needs to execute periodically.
        /// </summary>
        /// <param name="loc"></param>
        [HttpPost]
        public void CreateSession(BusinessLibrary.UIRequestClasses.UserLocationRequest loc)
        {
            string sessionID = Helpers.AuthenticationHelper.CreateSessionCookie(System.Web.HttpContext.Current, 120);
            Helpers.AuthenticationHelper.CreateSession(System.Web.HttpContext.Current, sessionID, loc.Coords, loc.IP);
        }
    }
}