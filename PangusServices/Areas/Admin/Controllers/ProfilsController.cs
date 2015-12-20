using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PangusServices.Infrastructure;
using System.Data.Entity;
using PangusServices.Filter;
using PangusServices.Models;

namespace PangusServices.Areas.Admin.Controllers
{
    [SelectedTab("servicii")]
    [Authorize(Roles = "Administrator, Iroda_Szentgyorgy, Iroda_Csikszereda")]
    public class ProfilsController : Controller
    {
        AppIdentityDbContext db = new AppIdentityDbContext();
        public ActionResult Index()
        {
            var profils = from s in db.Profils
                          orderby s.Name
                          select s;

            return View(profils.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.AnvelopeID = new SelectList(from s in db.Anvelopes orderby s.Marca select s, "AnvelopeID", "Marca");
            return View();
        }

        // POST: Admin/Profils/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Profil model)
        {
            var error = 0;
            var Name = db.Profils;
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
                db.Profils.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AnvelopeID = new SelectList(from s in db.Anvelopes orderby s.Marca select s, "AnvelopeID", "Marca");
            return View(model);
        }

        // GET: Admin/Profils/Edit/5
        public ActionResult Edit(int id)
        {
            var itemToEdit = db.Profils.Find(id);
            if(itemToEdit == null)
            {
                return HttpNotFound();
            }

            ViewBag.AnvelopeID = new SelectList(db.Anvelopes, "AnvelopeID", "Marca", itemToEdit.AnvelopeID);

            return View(itemToEdit);
        }

        // POST: Admin/Profils/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Profil model)
        {
            if(ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AnvelopeID = new SelectList(db.Anvelopes, "AnvelopeID", "Marca", model.AnvelopeID);
            return View(model);
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            var itemToDelete = db.Profils.Find(id);
            if(itemToDelete == null)
            {
                return HttpNotFound();
            }
            db.Profils.Remove(itemToDelete);
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
