using PangusServices.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PangusServices.Models;
using System.Data.Entity;
using PangusServices.Filter;

namespace PangusServices.Areas.Admin.Controllers
{
    [SelectedTab("servicii")]
    [Authorize(Roles = "Administrator, Iroda_Szentgyorgy, Iroda_Csikszereda")]
    public class CarTypesController : Controller
    {

        AppIdentityDbContext db = new AppIdentityDbContext();
        public ActionResult Index()
        {
            var carTypes = from s in db.CarTypes
                               orderby s.Name
                               select s;

            return View(carTypes.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CarType model)
        {
            var error = 0;
            var Name = db.CarTypes;
            if (model.Name == null)
            {
                error = 1;
            }
            foreach(var item in Name)
            {
                if (model.Name == item.Name)
                {
                    error = 1;
                }
            }
            if(ModelState.IsValid && error == 0)
            {
                db.CarTypes.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var entryToEdit = db.CarTypes.Find(id);
            if(entryToEdit == null)
            {
                return HttpNotFound();
            }

            return View(entryToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CarType model)
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
            var entryToDelete = db.CarTypes.Find(id);
            if(entryToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.CarTypes.Remove(entryToDelete);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}