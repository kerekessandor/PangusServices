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
    public class FirmeController : Controller
    {
        // GET: Admin/Firme
        AppIdentityDbContext db = new AppIdentityDbContext();
        public ActionResult Index()
        {
            var firme = from s in db.Firmes
                        orderby s.Name
                        select s;

            return View(firme.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Firme model)
        {
            var error = 0;
            var Name = db.Firmes;
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
                db.Firmes.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit (int id)
        {
            var itemToEdit = db.Firmes.Find(id);
            if (itemToEdit == null)
            {
                return HttpNotFound();
            }
            return View(itemToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Firme model, int id)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Details(int id)
        {
            var itemToView = db.Firmes.Find(id);
            if (itemToView == null)
            {
                return HttpNotFound();
            }
            return View(itemToView);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var itemToDelete = db.Firmes.Find(id);
            if (itemToDelete == null)
            {
                return HttpNotFound();
            }
            db.Firmes.Remove(itemToDelete);
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