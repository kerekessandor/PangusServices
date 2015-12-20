using PangusServices.Filter;
using PangusServices.Infrastructure;
using PangusServices.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PangusServices.Models;
using System.Data.Entity;
using PagedList;

namespace PangusServices.Areas.Admin.Controllers
{
    [SelectedTab("raport")]
    [Authorize(Roles = "Administrator, Iroda_Szentgyorgy, Iroda_Csikszereda")]
    public class RaportController : Controller
    {
        AppIdentityDbContext db = new AppIdentityDbContext();
        public ActionResult Index(DateTime? start, DateTime? end)
        {
            string isUser = "";
            var raport = db.Mains.ToList();
                    //.Include(x => x.CarDetail)
                    //.Include(x => x.PaymentMethod)
                    //.Include(x => x.CustomerServices)
                    //.Include(x => x.Customer).ToList();
                    //.Where(x => x.Customer.Date == DateTime.Today).ToList();

            ViewBag.tim = DateTime.Today.ToShortDateString();

            isUser = "(Sfantu Gheorghe - Miercurea Ciuc)";

            if (User.IsInRole("Iroda_Szentgyorgy") || User.IsInRole("Muhely_Szentgyorgy"))
            {
                raport = raport.Where(x => x.Customer.Date == DateTime.Today && x.Sfantu_MCiuc == true).ToList();
                isUser = "(Sfantu Gheorghe)";
            }
            else if (User.IsInRole("Iroda_Csikszereda") || User.IsInRole("Muhely_Csikszereda"))
            {
                raport = raport.Where(x => x.Customer.Date == DateTime.Today && x.Sfantu_MCiuc == false).ToList();
                isUser = "(Miercurea Ciuc)";
            }

            if (start != null && end !=null)
            {
                raport = db.Mains.Where(x => x.Customer.Date >= start && x.Customer.Date <= end).ToList();

                if (User.IsInRole("Iroda_Szentgyorgy") || User.IsInRole("Muhely_Szentgyorgy"))
                {
                    raport = raport.Where(x => x.Sfantu_MCiuc == true).ToList();
                }
                else if (User.IsInRole("Iroda_Csikszereda") || User.IsInRole("Muhely_Csikszereda"))
                {
                    raport = raport.Where(x => x.Sfantu_MCiuc == false).ToList();
                }
                
                DateTime kezdo = (DateTime)start;
                DateTime vegso = (DateTime)end;

                ViewBag.tim = "de la " + kezdo.ToShortDateString() +" pana la "+ vegso.ToShortDateString();
            }

            foreach (var item in raport)
            {
                if (item.PaymentMethodID == null)
                {
                    return RedirectToAction("Index", "Services");
                }
            }

            var raporti = from s in raport
                          group s by s.PaymentMethod.Name into dateGroup
                          select new PaymentNumberGroup()
                          {
                              PaymentMethod = dateGroup.Key,
                              PaymentCount = dateGroup.Count()
                          };

            ViewBag.totalSum = raport;
            ViewBag.isUser = isUser;

            return View(raporti.ToList());
        }

        public ActionResult RaportD(DateTime? start, DateTime? end, int? page, int? nmb, int? numList)
        {
            if (nmb == null)
            { 
                nmb = 0;
            }
            if (numList == null)
            {
                numList = 100;
            }
            string isUser = "";
            var raport = from s in db.Mains
                         select s;

            ViewBag.tim = DateTime.Today.ToShortDateString();

            isUser = "(Sfantu Gheorghe - Miercurea Ciuc)";
            if (User.IsInRole("Muhely_Szentgyorgy") || User.IsInRole("Iroda_Szentgyorgy"))
            {
                raport = from s in raport
                         where s.Customer.Date == DateTime.Today && s.Sfantu_MCiuc == true
                         select s;
                isUser = "(Sfantu Gheorghe)";
            }
            else if (User.IsInRole("Muhely_Csikszereda") || User.IsInRole("Iroda_Csikszereda"))
            {
                raport = from s in raport
                         where s.Customer.Date == DateTime.Today && s.Sfantu_MCiuc == false
                         select s;
                isUser = "(Miercurea Ciuc)";
            }

            if (start != null && end != null)
            {
                raport = from s in db.Mains
                         where s.Customer.Date >= start && s.Customer.Date <= end
                         select s;

                if (User.IsInRole("Iroda_Szentgyorgy") || User.IsInRole("Muhely_Szentgyorgy"))
                {
                    raport = raport.Where(x => x.Sfantu_MCiuc == true);
                }
                else if (User.IsInRole("Iroda_Csikszereda") || User.IsInRole("Muhely_Csikszereda"))
                {
                    raport = raport.Where(x => x.Sfantu_MCiuc == false);
                }

                DateTime kezdo = (DateTime)start;
                DateTime vegso = (DateTime)end;

                ViewBag.tim = "de la " + kezdo.ToShortDateString() + " pana la " + vegso.ToShortDateString();
            }

            foreach (var item in raport)
            {
                if (item.PaymentMethodID == null)
                {
                    return RedirectToAction("Index", "Services");
                }
            }

            var raporti = from s in raport
                          group s by s.PaymentMethod.Name into dateGroup
                          select new PaymentNumberGroup()
                          {
                              PaymentMethod = dateGroup.Key,
                              PaymentCount = dateGroup.Count()
                          };

            ViewBag.raporti = raporti;
            ViewBag.isUser = isUser;
            ViewBag.start = start;
            ViewBag.end = end;
            ViewBag.number = nmb;
            var numLst = new SelectList(new[] 
            {
                25,
                50,
                100,
                150,
                200
            });

            ViewBag.numList = numLst;
            ViewBag.fwdNumber = (int)numList;

            raport = from s in raport
                     orderby s.Customer.Date
                     select s;

            int pageSize = (int)numList;
            int pageNumber = (page ?? 1);
            return View(raport.ToPagedList(pageNumber, pageSize));
        }

       
        public ActionResult Print(DateTime? start, DateTime? end)
        {
            string isUser = "";
            var raport = from s in db.Mains
                         select s;

            ViewBag.tim = DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year;

            isUser = "(Sfantu Gheorghe - Miercurea Ciuc)";
            if (User.IsInRole("Muhely_Szentgyorgy") || User.IsInRole("Iroda_Szentgyorgy"))
            {
                raport = from s in raport
                         where s.Customer.Date == DateTime.Today && s.Sfantu_MCiuc == true
                         select s;
                isUser = "(Sfantu Gheorghe)";
            }
            else if (User.IsInRole("Muhely_Csikszereda") || User.IsInRole("Iroda_Csikszereda"))
            {
                raport = from s in raport
                         where s.Customer.Date == DateTime.Today && s.Sfantu_MCiuc == false
                         select s;
                isUser = "(Miercurea Ciuc)";
            }

            if (start != null && end != null)
            {
                raport = from s in db.Mains
                         where s.Customer.Date >= start && s.Customer.Date <= end
                         select s;

                if (User.IsInRole("Iroda_Szentgyorgy") || User.IsInRole("Muhely_Szentgyorgy"))
                {
                    raport = raport.Where(x => x.Sfantu_MCiuc == true);
                }
                else if (User.IsInRole("Iroda_Csikszereda") || User.IsInRole("Muhely_Csikszereda"))
                {
                    raport = raport.Where(x => x.Sfantu_MCiuc == false);
                }

                DateTime kezdo = (DateTime)start;
                DateTime vegso = (DateTime)end;

                ViewBag.tim = "de la " + kezdo.Date.Day + "/" + kezdo.Date.Month + "/" + kezdo.Date.Year + " pana la " + vegso.Date.Day + "/" + vegso.Date.Month + "/" + vegso.Date.Year;
            }

            foreach (var item in raport)
            {
                if (item.PaymentMethodID == null)
                {
                    return RedirectToAction("Index", "Services");
                }
            }

            var raporti = from s in raport
                          group s by s.PaymentMethod.Name into dateGroup
                          select new PaymentNumberGroup()
                          {
                              PaymentMethod = dateGroup.Key,
                              PaymentCount = dateGroup.Count()
                          };

            ViewBag.raporti = raporti;
            ViewBag.isUser = isUser;

            return PartialView("Print", raport.ToList());
        }

        public ActionResult RaportFirme()
        {
            ViewBag.PaymentMethodID = new SelectList(db.PaymentMethods, "PaymentMethodID", "Name");
            return View();
        }

        public ActionResult PrintRaportFirme(DateTime? start, DateTime? end, string firma, int PaymentMethodID)
        {
            if (start == null || end == null || firma == null)
            {
                return RedirectToAction("RaportFirme");
            }
            else
            {
                var raport = from s in db.Mains
                             where s.Customer.Client == firma && (s.Customer.Date >= start && s.Customer.Date <= end) && s.PaymentMethodID == PaymentMethodID
                             select s;

                DateTime kezdo = (DateTime)start;
                DateTime vegso = (DateTime)end;

                ViewBag.tim = "de la " + kezdo.Date.Day + "/" + kezdo.Date.Month + "/" + kezdo.Date.Year + " pana la " + vegso.Date.Day + "/" + vegso.Date.Month + "/" + vegso.Date.Year;
                ViewBag.Client = firma;

                return PartialView(raport.ToList());
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}