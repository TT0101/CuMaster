using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace CuMaster.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-theme-cumaster.css",
                "~/Content/font-awesome.css",
                "~/Content/CuMaster.css",
                "~/Content/DataTables/css/jquery.dataTables.min.css"
                ));


            //bundles.Add(new ScriptBundle("~/Scripts/sitejs").Include(
            //    "~/Scripts/jquery-{version}.js",
            //    "~/Scripts/jquery.validate.min.js",
            //    "~/Scripts/bootstrap.min.js",
            //    "~/Scripts/modernizr-2.8.3.js"
            //    ));
        }
    }
}