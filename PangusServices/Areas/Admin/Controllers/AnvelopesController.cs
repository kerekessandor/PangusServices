using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PangusServices.Infrastructure;
using PangusServices.Models;
using System.Data.Entity;
using PangusServices.Filter;

namespace PangusServices.Areas.Admin.Controllers
{
    [SelectedTab("servicii")]
    [Authorize(Roles = "Administrator, Iroda_Szentgyorgy, Iroda_Csikszereda")]
    public class AnvelopesController : Controller
    {

        AppIdentityDbContext db = new AppIdentityDbContext();
        public ActionResult Index()
        {
            var anvelope = from s in db.Anvelopes
                           orderby s.Marca
                           select s;

            return View(anvelope.ToList());
        }


        // GET: Admin/Anvelopes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Anvelopes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Anvelope model)
        {
            var error = 0;
            var Name = db.Anvelopes;
            if (model.Marca == null)
            {
                error = 1;
            }
            foreach (var item in Name)
            {
                if (model.Marca == item.Marca)
                {
                    error = 1;
                }
            }
            if (ModelState.IsValid && error == 0)
            {
                db.Anvelopes.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Admin/Anvelopes/Edit/5
        public ActionResult Edit(int id)
        {
            var itemToEdit = db.Anvelopes.Find(id);
            if(itemToEdit == null)
            {
                return HttpNotFound();
            }

            return View(itemToEdit);
        }

        // POST: Admin/Anvelopes/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Anvelope model)
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
            var entryToDelete = db.Anvelopes.Find(id);
            if (entryToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.Anvelopes.Remove(entryToDelete);
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
