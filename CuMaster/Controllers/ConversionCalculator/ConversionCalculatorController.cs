﻿using CuMaster.BusinessLibrary.ViewModels;
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
            BusinessLibrary.Library.CurrencyConversionLibrary ccLib = new BusinessLibrary.Library.CurrencyConversionLibrary();
            CurrencyConversionViewModel ccvModel = ccLib.GetCurrencyConversion();

            return View("ConversionCalculator", ccvModel);
        }

        [HttpPost]
        public JsonResult CurrencyChanged(CurrencyConversionViewModel res)
        {
            BusinessLibrary.Library.CurrencyConversionLibrary ccLib = new BusinessLibrary.Library.CurrencyConversionLibrary(res, false);
            ModelState.Clear();
            return Json(ccLib.GetCurrencyConversion(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FromValueChanged(CurrencyConversionViewModel res)
        {
            BusinessLibrary.Library.CurrencyConversionLibrary ccLib = new BusinessLibrary.Library.CurrencyConversionLibrary(res, false);
            return Json(ccLib.GetCurrencyConversion(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ToValueChanged(CurrencyConversionViewModel res)
        {
            BusinessLibrary.Library.CurrencyConversionLibrary ccLib = new BusinessLibrary.Library.CurrencyConversionLibrary(res, true);
            return Json(ccLib.GetCurrencyConversion(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ResetToDefault(CurrencyConversionViewModel res)
        {
            BusinessLibrary.Library.CurrencyConversionLibrary ccLib = new BusinessLibrary.Library.CurrencyConversionLibrary();
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