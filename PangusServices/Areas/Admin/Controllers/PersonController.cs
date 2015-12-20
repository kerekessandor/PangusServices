using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PangusServices.Models;
using PangusServices.Infrastructure;
using PagedList;
using PangusServices.Filter;

namespace PangusServices.Areas.Admin.Controllers
{
    [SelectedTab("servicii")]
    [Authorize(Roles = "Administrator, Iroda_Szentgyorgy, Iroda_Csikszereda")]
    public class PersonController : Controller
    {

        AppIdentityDbContext db = new AppIdentityDbContext();
        // GET: Admin/Person
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "lastName" ? "lastName_desc" : "lastName";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var person = from s in db.Customers
                         select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                person = person.Where(s => s.FirstName.ToUpper().Contains(searchString.ToUpper()) || s.LastName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    person = person.OrderByDescending(s => s.FirstName);
                    break;
                case "lastName":
                    person = person.OrderBy(s => s.LastName);
                    break;
                case "lastName_desc":
                    person = person.OrderByDescending(s => s.LastName);
                    break;
                default:
                    person = person.OrderBy(s => s.LastName);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(person.ToPagedList(pageNumber, pageSize));

        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    db.Customers.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            //ViewBag.CarId = new SelectList(db.Cars, "ID", "Name", model.CarId);
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}