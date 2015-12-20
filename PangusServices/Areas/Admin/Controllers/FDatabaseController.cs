using PangusServices.Filter;
using PangusServices.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace PangusServices.Areas.Admin.Controllers
{
    [SelectedTab("fdatabase")]
    [Authorize(Roles = "Administrator, Iroda_Szentgyorgy, Iroda_Csikszereda")]
    public class FDatabaseController : Controller
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


            var mains = from s in db.Mains
                        where s.PaymentMethodID != null
                        select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                mains = from s in db.Mains
                        where s.PaymentMethodID != null && (s.CarDetail.NrInmatricular.ToUpper().Contains(searchString.ToUpper()) ||
                        s.MainID.ToString() == searchString || s.Customer.FirstName.ToUpper().Contains(searchString.ToUpper())
                        || s.Customer.LastName.ToUpper().Contains(searchString.ToUpper()) || s.AnvelopeNoi.Profil.Anvelope.Marca.ToUpper().Contains(searchString.ToUpper())
                        || s.CarDetail.Rim.ToUpper().Contains(searchString.ToUpper()) || s.CarDetail.CarType.Name.ToUpper().Contains(searchString.ToUpper())
                        || s.AnvelopeNoi.Profil.Name.ToUpper().Contains(searchString.ToUpper()) || s.Customer.Client.ToUpper().Contains(searchString.ToUpper()) 
                        || s.Customer.Delegat.ToUpper().Contains(searchString.ToUpper()))
                        select s;
            }
            switch (sortOrder)
            {
                case "name_desc":
                    mains = mains.OrderByDescending(s => s.Customer.FirstName);
                    break;
                case "Name":
                    mains = mains.OrderBy(s => s.Customer.FirstName);
                    break;
                case "Date":
                    mains = mains.OrderBy(s => s.Customer.Date);
                    break;
                default:
                    mains = mains.OrderByDescending(s => s.Customer.Date);
                    break;
            }

            int pageSize = 50;
            int pageNumber = (page ?? 1);
            return View(mains.ToPagedList(pageNumber, pageSize));
        }

        public JsonResult alma(string term)
        {
            var licenceNumber = from s in db.Mains
                                where s.PaymentMethodID != null
                                select s;

            var filteredNumbers = licenceNumber.Where(x => x.CarDetail.NrInmatricular.Contains(term)).Select(y => y.CarDetail.NrInmatricular).First();

            return Json(filteredNumbers, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var itemToDelete = db.Mains.Find(id);
            var customerToDelete = db.Customers.Find(itemToDelete.CustomerID);
            var carToDelete = db.CarDetails.Find(itemToDelete.CarDetailID);

            if(itemToDelete == null)
            {
                return HttpNotFound();
            }
            db.Mains.Remove(itemToDelete);
            db.Customers.Remove(customerToDelete);
            db.CarDetails.Remove(carToDelete);
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