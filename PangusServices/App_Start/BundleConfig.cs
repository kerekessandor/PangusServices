using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace PangusServices.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/admin/styles")
                .Include("~/Content/Styles/jquery-ui.css")
                .Include("~/Content/Styles/bootstrap.css")
                .Include("~/Content/Styles/Admin.css")
                );

            bundles.Add(new StyleBundle("~/styles")
                .Include("~/Content/Styles/bootstrap.css")
                .Include("~/Content/Styles/jquery-ui.css")
                .Include("~/Content/Styles/Site.css")
                );

            bundles.Add(new ScriptBundle("~/admin/scripts")
                .Include("~/Scripts/jquery-2.1.4.js")
                .Include("~/Scripts/jquery-ui.js")
                .Include("~/Scripts/jquery.validate.js")
                .Include("~/Scripts/jquery.validate.unobtrusive.js")
                .Include("~/Scripts/bootstrap.js")
                .Include("~/Scripts/jquery-ui.js")
                );

            bundles.Add(new ScriptBundle("~/scripts")
                .Include("~/Scripts/jquery-2.1.4.js")
                .Include("~/Scripts/jquery-ui.js")
                .Include("~/Scripts/jquery.validate.js")
                .Include("~/Scripts/jquery.validate.unobtrusive.js")
                .Include("~/Scripts/bootstrap.js")
                );
        }
    }
}