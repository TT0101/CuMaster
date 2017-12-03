using CuMaster.BusinessLibrary.Library;
using CuMaster.BusinessLibrary.UIRequestClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuMaster.Controllers.UserManagement
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            return View("~/Views/UserManagement/Register.cshtml");
        }

        [HttpPost]
        public JsonResult CheckUserName(string UserName)
        {
            UserRegistrationLibrary lib = new UserRegistrationLibrary();
            if (!lib.IsUserNameTaken(UserName))
                return Json(new { Valid = true }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Valid = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CheckEmail(string Email)
        {
            UserRegistrationLibrary lib = new UserRegistrationLibrary();
            if (!lib.IsEmailTaken(Email))
                return Json(new { Valid = true }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Valid = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CheckPassword(CheckPasswordRequest cr)
        {
            UserRegistrationLibrary lib = new UserRegistrationLibrary();
            if (lib.DoesPasswordMeetRequirements(cr.Password, cr.UserName))
                return Json(new { Valid = true }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Valid = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RegisterUser(BusinessLibrary.Models.RegisterModel rm)
        {
            try
            {
                UserRegistrationLibrary lib = new UserRegistrationLibrary();
                if (!lib.IsUserNameTaken(rm.UserName) && !lib.IsEmailTaken(rm.Email) && lib.DoesPasswordMeetRequirements(rm.Password, rm.UserName))
                {
                    string sessionID = Helpers.AuthenticationHelper.GetSessionID(System.Web.HttpContext.Current);
                    DateTime expires = Helpers.AuthenticationHelper.GetCuMasterCookie(System.Web.HttpContext.Current.Request).Expires;
                    if (expires.Year == 1)
                        expires = DateTime.Now.AddHours(1);

                    lib.RegisterUser(rm, sessionID, expires);
                    return Json(new { StatusKey = "SUCCESS" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { StatusKey = "ERROR" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { StatusKey = "ERROR" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}