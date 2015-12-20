using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PangusServices.Filter;
using PangusServices.Infrastructure;
using System.Data.Entity;
using PangusServices.Models;
using PangusServices.ViewModels;
using System.Data.Entity.Infrastructure;
using System.Net;

namespace PangusServices.Areas.Admin.Controllers
{
    [SelectedTab("adminservices")]
    [Authorize(Roles = "Administrator, Iroda_Csikszereda , Iroda_Szentgyorgy")]
    public class ServicesController : Controller
    {
        AppIdentityDbContext db = new AppIdentityDbContext();
        public ActionResult Index(int? id)
        {
            var customer = from s in db.Mains
                           where (s.Sfantu_MCiuc == true) && (s.Customer.Date == DateTime.Today ||
                           s.PaymentMethodID == null || s.isDone == false)
                           select s;

            if (User.IsInRole("Iroda_Csikszereda"))
            {
                customer = from s in db.Mains
                           where (s.Sfantu_MCiuc == false) && (s.Customer.Date == DateTime.Today ||
                           s.PaymentMethodID == null || s.isDone == false)
                           select s;
            }
            if (User.IsInRole("Administrator"))
            {
                customer = from s in db.Mains
                           where (s.Sfantu_MCiuc == false || s.Sfantu_MCiuc == true) && (s.Customer.Date == DateTime.Today ||
                           s.PaymentMethodID == null || s.isDone == false)
                           select s;
            }
            return View(customer.ToList());
        }

        public ActionResult Details(int? id, string NrInmatricular)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Customer customer = db.Customers
                .Where(x => x.ID == id)
                .Single();

            if (customer == null)
            {
                return HttpNotFound();
            }

            ViewBag.carDetails = db.Mains.Where(s => s.CarDetail.NrInmatricular.Contains(NrInmatricular)).ToList();

            return View(customer);
        }

        public ActionResult EditOperator(int id)
        {
            var index = 0;
            var itemToEdit = db.Mains.Find(id);
            if (itemToEdit == null)
            {
                return HttpNotFound();
            }
            if (itemToEdit.OperatorID != null)
            {
                index = (int)itemToEdit.OperatorID;
            }
            ViewBag.OperatorID = new SelectList(db.Operators, "OperatorID", "Name", index);
            return View(itemToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOperator(Main model, int id)
        {
            var operatorSave = db.Operators.Find(model.OperatorID);
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                model.OperatorName = operatorSave.Name;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OperatorID = new SelectList(db.Operators, "OperatorID", "Name", model.OperatorID);
            return View(model);
        }

        public ActionResult Print(int id)
        {
            var customerToPrint = db.Customers.Find(id);
            return PartialView(customerToPrint);
        }

        private void PopulateAssignedServiciiData(CustomerService customer)
        {
            var allServices = db.PangusServiciis;
            var viewModel = new List<AssignedServicesData>();
            foreach (var service in allServices)
            {
                viewModel.Add(new AssignedServicesData
                {
                    ServiciiId = service.ID,
                    ServiciiName = service.Name
                });
            }
            ViewBag.Serviciis = viewModel;
        }


        public ActionResult AddPayment(int id)
        {
            var entityToAdd = db.Mains.Find(id);
            if (entityToAdd == null)
            {
                return HttpNotFound();
            }
            ViewBag.PaymentMethodID = new SelectList(from s in db.PaymentMethods orderby s.Name select s, "PaymentMethodID", "Name");
            return View(entityToAdd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPayment(int id, Main model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PaymentMethodID = new SelectList(from s in db.PaymentMethods orderby s.Name select s, "PaymentMethodID", "Name");
            return View(model);
        }

        public ActionResult EditPayment(int id)
        {
            var paymentToEdit = db.Mains.Find(id);
            if (paymentToEdit == null)
            {
                return HttpNotFound();
            }
            ViewBag.PaymentMethodID = new SelectList(db.PaymentMethods, "PaymentMethodID", "Name", paymentToEdit.PaymentMethodID);
            return View(paymentToEdit);
        }

        [HttpPost]
        public ActionResult EditPayment(int id, Main model)
        {

            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PaymentMethodID = new SelectList(db.PaymentMethods, "PaymentMethodID", "Name", model.PaymentMethodID);
            return View(model);

        }

        public JsonResult SelectFirmes(string term)
        {
            List<string> firmes;
            firmes = db.Firmes.Where(x => x.Name.StartsWith(term)).Select(y => y.Name).ToList();
            return Json(firmes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditInfo(int id)
        {
            Customer customerToEdit = db.Customers.Find(id);
            if (customerToEdit == null)
            {
                return HttpNotFound();
            }
            return View(customerToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditInfo(Customer model, int id)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = id });
            }
            return View(model);
        }

        public ActionResult EditCar(int id, int custID, int mainID)
        {
            var main = db.Mains.Find(mainID);
            ViewBag.IsDepo = main.Depozitare.IsDepozitare;
            var carToEdit = db.CarDetails.Find(id);
            if (carToEdit == null)
            {
                return HttpNotFound();
            }
            ViewBag.backToEdit = custID;
            return View(carToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCar(CarDetail model, int custID, int mainID)
        {
            var main = db.Mains.Find(mainID);
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = custID });
            }
            ViewBag.IsDepo = main.Depozitare.IsDepozitare;
            ViewBag.backToEdit = custID;
            return View(model);
        }

        public ActionResult EditAnvelopa(int id, int custID)
        {
            var anvelopeID = 0;
            var profilID = 0;
            var anvelopToEdit = db.AnvelopeNois.Find(id);

            if (anvelopToEdit.NoiRulate)
            {
                if (anvelopToEdit.ProfilID != null)
                {
                    anvelopeID = anvelopToEdit.Profil.AnvelopeID;
                    profilID = (int)anvelopToEdit.ProfilID;
                }
            }

            if (anvelopToEdit == null)
            {
                return HttpNotFound();
            }
            ViewBag.backToEdit = custID;
            ViewBag.ProfilID = new SelectList(from s in db.Profils orderby s.Name select s, "ProfilID", "Name", profilID);
            ViewBag.AnvelopeID = new SelectList(from s in db.Anvelopes orderby s.Marca select s, "AnvelopeID", "Marca", anvelopeID);
            return View(anvelopToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAnvelopa(AnvelopeNoi model, int custID)
        {
            if (ModelState.IsValid)
            {
                model.NoiRulate = true;
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = custID });
            }
            ViewBag.backToEdit = custID;
            ViewBag.ProfilID = new SelectList(db.Profils, "ProfilID", "Name", model.ProfilID);
            ViewBag.AnvelopeID = new SelectList(db.Anvelopes, "AnvelopeID", "Marca", model.Profil.AnvelopeID);
            return View(model);
        }

        public ActionResult EditDepozitare(int id, int custID)
        {
            var anvelopeID = 0;
            var profilID = 0;
            var depoToEdit = db.Depozitares.Find(id);

            if (depoToEdit == null)
            {
                return HttpNotFound();
            }
            ViewBag.backToDetails = custID;
            if (depoToEdit.IsDepozitare)
            {
                anvelopeID = depoToEdit.Profil.AnvelopeID;
                profilID = (int)depoToEdit.ProfilID;
            }
            ViewBag.AnvelopeID = new SelectList(from s in db.Anvelopes orderby s.Marca select s, "AnvelopeID", "Marca", anvelopeID);
            ViewBag.ProfilID = new SelectList(from s in db.Profils orderby s.Name select s, "ProfilID", "Name", profilID);

            return View(depoToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDepozitare(Depozitare model, int id, int custID, int carDetail)
        {
            var customerToFind = db.Customers.Find(custID);
            var carDetaiToFind = db.CarDetails.Find(carDetail);

            if (ModelState.IsValid)
            {
                //model.FirstName = customerToFind.FirstName;
                //model.IsDepozitare = true;
                //model.LastName = customerToFind.LastName;
                //model.NrInmatricuare = carDetaiToFind.NrInmatricular;
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = custID });
            }
            ViewBag.backToDetails = custID;
            ViewBag.AnvelopeID = new SelectList(from s in db.Anvelopes orderby s.Marca select s, "AnvelopeID", "Marca", model.Profil.AnvelopeID);
            ViewBag.ProfilID = new SelectList(from s in db.Profils orderby s.Name select s, "ProfilID", "Name", model.ProfilID);
            return View(model);
        }

        public ActionResult editServicii(int id, int custID)
        {
            var serviciiToEdit = db.CustomerServices.Find(id);
            if (serviciiToEdit == null)
            {
                return HttpNotFound();
            }
            ViewBag.backToDetails = custID;
            return View(serviciiToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult editServicii(int custID, CustomerService model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = custID });
            }
            ViewBag.backToDetails = custID;
            return View(model);
        }

        [HttpPost]
        public ActionResult isDone(int id)
        {
            var itemDone = db.Mains.Find(id);
            if (itemDone == null)
            {
                return HttpNotFound();
            }

            itemDone.isDone = true;
            itemDone.Sfantu_MCiuc = itemDone.Sfantu_MCiuc;
            db.Entry(itemDone).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var itemToDelete = db.Mains.Find(id);
            var customerToDelete = db.Customers.Find(itemToDelete.CustomerID);
            var carToDelete = db.CarDetails.Find(itemToDelete.CarDetailID);

            if (itemToDelete == null)
            {
                return HttpNotFound();
            }
            db.Mains.Remove(itemToDelete);
            db.Customers.Remove(customerToDelete);
            db.CarDetails.Remove(carToDelete);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult editMentiuni(int id, int custID)
        {
            var itemToEdit = db.CarDetails.Find(id);
            if (itemToEdit == null)
            {
                return HttpNotFound();
            }
            ViewBag.backToDetails = custID;
            return View(itemToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult editMentiuni(CarDetail model, int custID)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = custID });
            }
            return View(model);
        }

        public ActionResult Discount(int id, int custId)
        {
            var itemToDiscount = db.Mains.Find(id);
            ViewBag.custId = custId;
            if (itemToDiscount == null)
            {
                return HttpNotFound();
            }
            return View(itemToDiscount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Discount(Main model, int custId)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }

}