using CuMaster.BusinessLibrary.Classes.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuMaster.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View("ErrorView");
        }

        public ActionResult Error(int statusCode, Exception ex)
        {

            Response.StatusCode = statusCode;
            Models.ErrorModel e;
            if (Session["_ERROR_"] != null && (Session["_ERROR_"].GetType() == typeof(Exception) || Session["_ERROR_"].GetType() == typeof(HttpException)))
            {
                e = new Models.ErrorModel((Exception)Session["_ERROR_"]);
            }
            else
            {
                e = new Models.ErrorModel(ex);
            }

            return View("ErrorView", e);
        }
    }
}