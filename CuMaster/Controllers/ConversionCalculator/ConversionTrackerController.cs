using HelperFramework.UI.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using CuMaster.BusinessLibrary.ViewModels;

namespace CuMaster.Controllers.ConversionCalculator
{
    public class ConversionTrackerController : Controller
    {
        // GET: ConversionTracker
        public ActionResult Index()
        {
            return View("~/Views/ConversionCalculator/ConversionTracker.cshtml");
        }

        [HttpPost]
        public JsonResult GetTrackerEntries(DataTableParams dtParams)
        {
            BusinessLibrary.Library.CurrencyConversionTrackerLibrary ctLib = new BusinessLibrary.Library.CurrencyConversionTrackerLibrary();
            return Json(ctLib.GetTrackerListForUser(dtParams).EntryListDataTable, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveNewTrackerEntry(ConversionTrackerViewModel tvm)
        {
            BusinessLibrary.Library.CurrencyConversionTrackerLibrary ctLib = new BusinessLibrary.Library.CurrencyConversionTrackerLibrary();
            try
            {
                ctLib.SaveNewEntry(tvm);
                return Json(new { StatusKey = "SUCCESS" });
            }
            catch
            {
                return Json(new { StatusKey = "ERROR" });
            }
        }

        [HttpPost]
        public JsonResult SaveAutoUpdateChange(BusinessLibrary.UIRequestClasses.TrackerUpdateRequest entryRowObject)
        {

            BusinessLibrary.Library.CurrencyConversionTrackerLibrary ctLib = new BusinessLibrary.Library.CurrencyConversionTrackerLibrary();
            try
            {
                ctLib.SaveAutoUpdateChange(entryRowObject.entryID);
                return Json(new { StatusKey = "SUCCESS", rowID = entryRowObject.rowID });
            }
            catch
            {
                return Json(new { StatusKey = "ERROR", rowID = entryRowObject.rowID });
            }

        }

        [HttpPost]
        public JsonResult DeleteEntry(BusinessLibrary.UIRequestClasses.TrackerUpdateRequest entryRowObject)
        {
            BusinessLibrary.Library.CurrencyConversionTrackerLibrary ctLib = new BusinessLibrary.Library.CurrencyConversionTrackerLibrary();
            try
            {
                ctLib.DeleteEntry(entryRowObject.entryID);
                return Json(new { StatusKey = "SUCCESS", rowID = entryRowObject.rowID });
            }
            catch
            {
                return Json(new { StatusKey = "ERROR", rowID = entryRowObject.rowID });
            }
        }

        [HttpPost]
        public JsonResult DeleteAllEntries()
        {
            BusinessLibrary.Library.CurrencyConversionTrackerLibrary ctLib = new BusinessLibrary.Library.CurrencyConversionTrackerLibrary();
            try
            {
                ctLib.DeleteAllEntriesForUser();
                return Json(new { StatusKey = "SUCCESS"});
            }
            catch
            {
                return Json(new { StatusKey = "ERROR" });
            }
        }
    }
}