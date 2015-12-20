using PangusServices.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PangusServices.Areas.Admin.Controllers
{
    [SelectedTab("info")]
    [Authorize(Roles = "Administrator, Iroda_Szentgyorgy, Iroda_Csikszereda")]
    public class InfoController : Controller
    {
        // GET: Admin/Info
        public ActionResult Index()
        {
            return View();
        }
    }
}