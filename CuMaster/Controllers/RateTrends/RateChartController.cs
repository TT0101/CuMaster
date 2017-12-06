using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuMaster.Controllers.RateTrends
{
    public class RateChartController : Controller
    {
        // GET: RateChart
        public ActionResult Index()
        {
            return View("~/Views/RateTrends/RateChart.cshtml");
        }

        public JsonResult GetRateTrendData(BusinessLibrary.UIRequestClasses.RateChartDataRequest rcd)
        {
            BusinessLibrary.Library.RateTrendsLibrary rtLib = new BusinessLibrary.Library.RateTrendsLibrary();
            return Json(new { Data = rtLib.GetHistoricalRates(rcd.CurrencyFrom, rcd.CurrencyTo, rcd.Days), CurrencyTo = rcd.CurrencyTo }, JsonRequestBehavior.AllowGet);
        }
    }
}