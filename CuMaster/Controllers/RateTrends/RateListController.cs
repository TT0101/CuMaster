using CuMaster.BusinessLibrary.Classes.Session;
using CuMaster.BusinessLibrary.ViewModels;
using HelperFramework.UI.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuMaster.Controllers.RateTrends
{
    public class RateListController : Controller
    {
        // GET: RateList
        public ActionResult Index()
        {
            Session session = Helpers.AuthenticationHelper.GetSession(System.Web.HttpContext.Current);
            BusinessLibrary.Library.RateTrendsLibrary rtLib = new BusinessLibrary.Library.RateTrendsLibrary();
            return View("~/Views/RateTrends/RateList.cshtml", rtLib.GetInitialRateListSettings(session));
        }

        [HttpPost]
        public JsonResult GetRatesForBase(DataTableParams Params, string BaseCurrency)
        {
            BusinessLibrary.Library.RateTrendsLibrary rtLib = new BusinessLibrary.Library.RateTrendsLibrary();
            return Json(rtLib.GetRatesForBase(Params, BaseCurrency), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CurrencyChanged(BusinessLibrary.ViewModels.RateTrendsViewModel rt)
        {
            //RateListViewModel rlv = new RateListViewModel
            //{
            //    BaseCurrency = rt.BaseCurrency
            //};
            //return View("~/Views/RateTrends/RateList.cshtml", rlv);
            return Json(new { BaseCurrency = rt.BaseCurrency }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult TimeSpanChange(BusinessLibrary.ViewModels.RateTrendsViewModel rt)
        //{
        //    return Json(new { });
        //}

    }
}