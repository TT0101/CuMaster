using CuMaster.BusinessLibrary.Classes.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuMaster.Controllers.RateTrends
{
    public class RateTrendsController : Controller
    {
        // GET: RateTrends
        public ActionResult Index()
        {
            Session session = Helpers.AuthenticationHelper.GetSession(System.Web.HttpContext.Current);

            return View("~/Views/RateTrends/RateTrends.cshtml", new BusinessLibrary.Library.RateTrendsLibrary().GetInitialRateSettings(session));
        }
    }
}