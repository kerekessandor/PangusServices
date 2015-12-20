using PangusServices.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PangusServices.Filter
{
    public class DepozitareFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var context = new AppIdentityDbContext();

            var depozitare = 0;
            var depozitareMc = 0;

            foreach (var item in context.Depozitares)
            {
                if (item.IsDepozitare == true)
                {
                    if (item.Sfantu_Mciuc == true)
                    {
                        depozitare++;
                    }
                    else
                    {
                        depozitareMc++;
                    }
                }
            }
            filterContext.Controller.ViewBag.DepozitaresMc = depozitareMc;
            filterContext.Controller.ViewBag.Depozitares = depozitare;

        }
    }
}