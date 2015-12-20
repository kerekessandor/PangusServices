using PangusServices.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PangusServices.Areas.Admin.Controllers;

namespace PangusServices
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var namespaces = new[] { typeof(FreeServicesController).Namespace }; // specifieing the simple namespace(not the admin)

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            routes.MapRoute("Home", "", new { controller = "Login", action = "Login" }, namespaces);
            routes.MapRoute("freeServices", "freeServices", new { controller = "FreeServices", action = "Index" }, namespaces);
            routes.MapRoute("Create", "create", new { controller = "FreeServices", action = "Create" }, namespaces);
            routes.MapRoute("PJCreate", "pjcreate", new { controller = "FreeServices", action = "PJCreate" }, namespaces);
            routes.MapRoute("Login", "login", new { controller = "Login", action = "Login" }, namespaces);
            routes.MapRoute("Logout", "logout", new { controller = "Login", action = "Logout" }, namespaces);
            routes.MapRoute("SelectProfils", "SelectProfils", new { controller = "FreeServices", action = "SelectProfils" }, namespaces);
            routes.MapRoute("SelectFirmes", "SelectFirmes", new { controller = "FreeServices", action = "SelectFirmes" }, namespaces);
        }
    }
}
