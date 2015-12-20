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
    public class OperatorsController : Controller
    {
        AppIdentityDbContext db = new AppIdentityDbContext();
        // GET: Admin/Operators
        public ActionResult Index()
        {
            return View(db.Operators.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create (Operator model)
        {
            if (ModelState.IsValid)
            {
                db.Operators.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var itemToEdit = db.Operators.Find(id);
            if (itemToEdit == null)
            {
                return HttpNotFound();
            }
            return View(itemToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Operator model)
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
            var itemToDelete = db.Operators.Find(id);
            if (itemToDelete == null)
            {
                return HttpNotFound();
            }
            db.Operators.Remove(itemToDelete);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}