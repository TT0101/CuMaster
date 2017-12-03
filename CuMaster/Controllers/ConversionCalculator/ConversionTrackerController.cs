using HelperFramework.UI.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using CuMaster.BusinessLibrary.ViewModels;
using CuMaster.BusinessLibrary.Classes.Session;

namespace CuMaster.Controllers.ConversionCalculator
{
    public class ConversionTrackerController : Controller
    {
        private Session currentSession;
        
        // GET: ConversionTracker
        public ActionResult Index()
        {
            //this.currentSession = Helpers.AuthenticationHelper.GetSession(System.Web.HttpContext.Current);
            return View("~/Views/ConversionCalculator/ConversionTracker.cshtml");
        }

        [HttpPost]
        public JsonResult GetTrackerEntries(DataTableParams dtParams)
        {
            Session currentSession = Helpers.AuthenticationHelper.GetSession(System.Web.HttpContext.Current);
            BusinessLibrary.Library.CurrencyConversionTrackerLibrary ctLib = new BusinessLibrary.Library.CurrencyConversionTrackerLibrary(System.Web.HttpContext.Current, currentSession);
            return Json(ctLib.GetTrackerListForUser(dtParams).EntryListDataTable, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveNewTrackerEntry(ConversionTrackerViewModel tvm)
        {
            Session currentSession = Helpers.AuthenticationHelper.GetSession(System.Web.HttpContext.Current);

            BusinessLibrary.Library.CurrencyConversionTrackerLibrary ctLib = new BusinessLibrary.Library.CurrencyConversionTrackerLibrary(System.Web.HttpContext.Current, currentSession);
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
            Session currentSession = Helpers.AuthenticationHelper.GetSession(System.Web.HttpContext.Current);
            BusinessLibrary.Library.CurrencyConversionTrackerLibrary ctLib = new BusinessLibrary.Library.CurrencyConversionTrackerLibrary(System.Web.HttpContext.Current, currentSession);
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
            Session currentSession = Helpers.AuthenticationHelper.GetSession(System.Web.HttpContext.Current);
            BusinessLibrary.Library.CurrencyConversionTrackerLibrary ctLib = new BusinessLibrary.Library.CurrencyConversionTrackerLibrary(System.Web.HttpContext.Current, currentSession);
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
            Session currentSession = Helpers.AuthenticationHelper.GetSession(System.Web.HttpContext.Current);
            BusinessLibrary.Library.CurrencyConversionTrackerLibrary ctLib = new BusinessLibrary.Library.CurrencyConversionTrackerLibrary(System.Web.HttpContext.Current, currentSession);
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