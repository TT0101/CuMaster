using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuMaster.Controllers.UserManagement
{
    public class LoginRegisterController : Controller
    {
        // GET: LoginRegister
        public ActionResult Index()
        {
            return View("~/Views/UserManagement/LoginRegister.cshtml");
        }
    }
}