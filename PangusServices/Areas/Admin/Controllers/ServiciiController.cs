using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PangusServices.Models;
using PangusServices.Infrastructure;
using PagedList;
using System.Data.Entity;
using System.Data;
using PangusServices.Filter;

namespace PangusServices.Areas.Admin.Controllers
{
    [SelectedTab("servicii")]
    [Authorize(Roles = "Administrator, Iroda_Szentgyorgy, Iroda_Csikszereda")]
    public class ServiciiController : Controller
    {
        // GET: Admin/Servicii
        AppIdentityDbContext db = new AppIdentityDbContext();
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "Price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var person = from s in db.PangusServiciis
                         select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                person = person.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper()) || s.Price.ToString().Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    person = person.OrderByDescending(s => s.Name);
                    break;
                case "Price":
                    person = person.OrderBy(s => s.Price);
                    break;
                case "Price_desc":
                    person = person.OrderByDescending(s => s.Price);
                    break;
                default:
                    person = person.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 50;
            int pageNumber = (page ?? 1);
            return View(person.ToPagedList(pageNumber, pageSize));

        }

        public JsonResult GetServices(string term)
        {
            List<string> servicii;
            servicii = db.PangusServiciis.Where(x => x.Name.StartsWith(term)).Select(y => y.Name).ToList();
            return Json(servicii, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            ViewBag.dimTypeID = new SelectList(db.DimTypes, "dimTypeID", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(PangusServicii model)
        {
            if(ModelState.IsValid)
            {
                db.PangusServiciis.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var entityToEdit = db.PangusServiciis.Find(id);
            ViewBag.dimTypeID = new SelectList(db.DimTypes, "dimTypeID", "Name", entityToEdit.dimTypeID);
            return View(entityToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PangusServicii model)
        {
            if(ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.dimTypeID = new SelectList(db.DimTypes, "dimTypeID", "Name", model.dimTypeID);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var entityToDelete = db.PangusServiciis.Find(id);
            if (entityToDelete == null)
            {
                return HttpNotFound();
            }
            try
            {
                db.PangusServiciis.Remove(entityToDelete);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Nem lehet kitörölni, kérem próbálja újra");
            }
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}