using CuMaster.BusinessLibrary.Classes.Session;
using CuMaster.BusinessLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuMaster.Controllers.ConversionCalculator
{
    public class ConversionCalculatorController : Controller
    {
        // GET: ConversionCalculator
        public ActionResult Index()
        {
            Session session = Helpers.AuthenticationHelper.GetSession(System.Web.HttpContext.Current);
           
            BusinessLibrary.Library.CurrencyConversionLibrary ccLib = new BusinessLibrary.Library.CurrencyConversionLibrary(session);
            CurrencyConversionViewModel ccvModel = ccLib.GetCurrencyConversion();

            return View("ConversionCalculator", ccvModel);
        }

        [HttpPost]
        public JsonResult CurrencyChanged(CurrencyConversionViewModel res)
        {
            Session session = Helpers.AuthenticationHelper.GetSession(System.Web.HttpContext.Current);

            BusinessLibrary.Library.CurrencyConversionLibrary ccLib = new BusinessLibrary.Library.CurrencyConversionLibrary(session, res, false);
            ModelState.Clear();
            return Json(ccLib.GetCurrencyConversion(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FromValueChanged(CurrencyConversionViewModel res)
        {
            Session session = Helpers.AuthenticationHelper.GetSession(System.Web.HttpContext.Current);

            BusinessLibrary.Library.CurrencyConversionLibrary ccLib = new BusinessLibrary.Library.CurrencyConversionLibrary(session, res, false);
            return Json(ccLib.GetCurrencyConversion(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ToValueChanged(CurrencyConversionViewModel res)
        {
            Session session = Helpers.AuthenticationHelper.GetSession(System.Web.HttpContext.Current);

            BusinessLibrary.Library.CurrencyConversionLibrary ccLib = new BusinessLibrary.Library.CurrencyConversionLibrary(session, res, true);
            return Json(ccLib.GetCurrencyConversion(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ResetToDefault(CurrencyConversionViewModel res)
        {
            BusinessLibrary.Library.CurrencyConversionLibrary ccLib = new BusinessLibrary.Library.CurrencyConversionLibrary(Helpers.AuthenticationHelper.GetSession(System.Web.HttpContext.Current));
            CurrencyConversionViewModel ccvModel = ccLib.GetCurrencyConversion();

            return Json(ccLib.GetCurrencyConversion(), JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public JsonResult GetCurrencyConversionInformation()
        //{
        //    BusinessLibrary.Library.CurrencyConversionLibrary ccLib = new BusinessLibrary.Library.CurrencyConversionLibrary();
        //    BusinessLibrary.ViewModels.CurrencyConversionViewModel ccvModel = ccLib.GetInitalCurrencyConversion();

        //    return Json(ccvModel, JsonRequestBehavior.AllowGet);
        //}
    }
}