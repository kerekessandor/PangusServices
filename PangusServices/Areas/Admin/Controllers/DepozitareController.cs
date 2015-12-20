using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PangusServices.Infrastructure;
using PangusServices.Filter;
using PangusServices.Models;
using System.Data.Entity;
using PagedList;

namespace PangusServices.Areas.Admin.Controllers
{
    [SelectedTab("Depozitare")]
    public class DepozitareController : Controller
    {
        AppIdentityDbContext db = new AppIdentityDbContext();
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "Date" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "Name_desc" : "Name";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var depos = from s in db.Depozitares
                        where s.IsDepozitare == true 
                        select s;

            if (User.IsInRole("Iroda_Szentgyorgy") || User.IsInRole("Muhely_Szentgyorgy"))
            {
                depos = from s in depos
                        where s.Sfantu_Mciuc == true
                        select s;
            }
            if (User.IsInRole("Iroda_Csikszereda") || User.IsInRole("Muhely_Csikszereda"))
            {
                depos = from s in depos
                        where s.Sfantu_Mciuc == false
                        select s;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                depos = from s in depos
                        where s.IsDepozitare == true && ((s.NrInmatricuare.ToUpper().Contains(searchString.ToUpper())) 
                        || (s.FirstName.ToUpper().Contains(searchString.ToUpper())) || (s.LastName.ToUpper().Contains(searchString.ToUpper()))
                        || (s.Profil.Anvelope.Marca.ToUpper().Contains(searchString.ToUpper())))
                        select s;
            }
            switch (sortOrder)
            {
                case "name_desc":
                    depos = depos.OrderByDescending(s => s.FirstName);
                    break;
                case "Name":
                    depos = depos.OrderBy(s => s.FirstName);
                    break;
                case "Date":
                    depos = depos.OrderBy(s => s.Data);
                    break;
                default:
                    depos = depos.OrderByDescending(s => s.Data);
                    break;
            }

            int pageSize = 30;
            int pageNumber = (page ?? 1);
            return View(depos.ToPagedList(pageNumber, pageSize));
        }

        public JsonResult GetServices(string term)
        {
            var licenceNumber = from s in db.Depozitares
                                where s.IsDepozitare == true
                                select s;

            var filteredNumbers = licenceNumber.Where(x => x.NrInmatricuare.StartsWith(term)).Select(y => y.NrInmatricuare).ToList();

            return Json(filteredNumbers, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Print(int id)
        {
            var depoToPrint = db.Depozitares.Find(id);
            return PartialView(depoToPrint);
        }

        public ActionResult Create()
        {
            ViewBag.AnvelopeID = new SelectList(from s in db.Anvelopes orderby s.Marca select s, "AnvelopeID", "Marca");
            ViewBag.ProfilID = new SelectList(from s in db.Profils orderby s.Name select s, "ProfilID", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Depozitare model)
        {
            if(ModelState.IsValid)
            {
                model.IsDepozitare = true;
                if (User.IsInRole("Iroda_Szentgyorgy") || User.IsInRole("Muhely_Szentgyorgy"))
                {
                    model.Sfantu_Mciuc = true;
                }
                db.Depozitares.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AnvelopeID = new SelectList(db.Anvelopes, "AnvelopeID", "Marca");
            ViewBag.ProfilID = new SelectList(db.Profils, "ProfilID", "Name");
            return View(model);
        }

        
        public ActionResult Edit(int id)
        {
            var itemToEdit = db.Depozitares.Find(id);
            if(itemToEdit == null)
            {
                return HttpNotFound();
            }
            var profil = 0;
            var anvelope = 0;
            if(itemToEdit.ProfilID != null)
            {
                profil = (int)itemToEdit.ProfilID;
                anvelope = itemToEdit.Profil.AnvelopeID;
            }
            ViewBag.ProfilID = new SelectList(db.Profils, "ProfilID", "Name", profil);
            ViewBag.AnvelopeID = new SelectList(db.Anvelopes, "AnvelopeID", "Marca", anvelope);
            return View(itemToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Depozitare model)
        {

            if(ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProfilID = new SelectList(db.Profils, "ProfilID", "Name", model.ProfilID);
            ViewBag.AnvelopeID = new SelectList(db.Anvelopes, "AnvelopeID", "Marca", model.Profil.AnvelopeID);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var itemToDelete = db.Depozitares.Find(id);
            if(itemToDelete == null)
            {
                return HttpNotFound();
            }
            itemToDelete.IsDepozitare = false;
            itemToDelete.LastName = null;
            itemToDelete.FirstName = null;
            itemToDelete.Dimensiune = null;
            itemToDelete.Cantitate = null;
            db.Entry(itemToDelete).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}