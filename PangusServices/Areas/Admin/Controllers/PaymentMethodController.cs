using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PangusServices.Infrastructure;
using PangusServices.Models;
using PangusServices.Filter;
using System.Data.Entity;

namespace PangusServices.Areas.Admin.Controllers
{
    [SelectedTab("servicii")]
    [Authorize(Roles = "Administrator, Iroda_Szentgyorgy, Iroda_Csikszereda")]
    public class PaymentMethodController : Controller
    {
        AppIdentityDbContext db = new AppIdentityDbContext();
        public ActionResult Index()
        {
            var payments = from s in db.PaymentMethods
                           orderby s.Name
                           select s;

            return View(payments.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PaymentMethod model)
        {
            var error = 0;
            var Name = db.PaymentMethods;
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
                db.PaymentMethods.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var itemToEdit = db.PaymentMethods.Find(id);
            if(itemToEdit == null)
            {
                return HttpNotFound();
            }
            return View(itemToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PaymentMethod model)
        {
            if(ModelState.IsValid)
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
            var itemToDelete = db.PaymentMethods.Find(id);
            if(itemToDelete == null)
            {
                return HttpNotFound();
            }
            db.PaymentMethods.Remove(itemToDelete);
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