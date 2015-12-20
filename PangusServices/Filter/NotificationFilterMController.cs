using PangusServices.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PangusServices.Filter
{
    public class NotificationFilterMController : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var context = new AppIdentityDbContext();
            var notificationsMc = 0;

            foreach (var item in context.Mains)
            {
                if (item.PaymentMethodID == null && item.Sfantu_MCiuc == false)
                {
                    notificationsMc++;
                }
            }

            filterContext.Controller.ViewBag.NotificationsMc = notificationsMc;
        }
    }
}