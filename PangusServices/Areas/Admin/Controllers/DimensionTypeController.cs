using PangusServices.Filter;
using PangusServices.Infrastructure;
using PangusServices.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PangusServices.Areas.Admin.Controllers
{
    [SelectedTab("servicii")]
    [Authorize(Roles = "Administrator, Iroda_Szentgyorgy, Iroda_Csikszereda")]
    public class DimensionTypeController : Controller
    {
        AppIdentityDbContext db = new AppIdentityDbContext();
        public ActionResult Index()
        {
            var dimTypes = from s in db.DimTypes
                           orderby s.dimTypeID
                           select s;
            return View(dimTypes.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(dimType model)
        {
            var error = 0;
            var Name = db.DimTypes;
            if (model.Name == null)
            {
                error = 1;
            }
            foreach (var item in Name)
            {
                if (model.Name == item.Name)
                {
                    error = 1;
                }
            }
            if (ModelState.IsValid && error == 0)
            {
                db.DimTypes.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var itemToEdit = db.DimTypes.Find(id);
            if (itemToEdit == null)
            {
                return HttpNotFound();
            }
            return View(itemToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, dimType model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var itemToDelete = db.DimTypes.Find(id);
            if (itemToDelete == null)
            {
                return HttpNotFound();
            }
            db.DimTypes.Remove(itemToDelete);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}