using PangusServices.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PangusServices.Filter
{
    public class NotificationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var context = new AppIdentityDbContext();
            var notificationsSf = 0;

            foreach(var item in context.Mains)
            {
                if(item.PaymentMethodID == null && item.Sfantu_MCiuc == true)
                {
                    notificationsSf++;
                }
            }

            filterContext.Controller.ViewBag.Notifications = notificationsSf;
        }
    }
}