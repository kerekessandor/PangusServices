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

namespace PangusServices.Controllers
{
    [SelectedTab("services")]
    public class FreeServicesController : Controller
    {
        AppIdentityDbContext db = new AppIdentityDbContext();
        public ActionResult Index()
        {
            var isDone = from s in db.Mains
                         where s.Customer.Date == DateTime.Today && s.isDone == true && s.Sfantu_MCiuc == false
                         orderby s.Customer.Date descending
                         select s;

            if (User.IsInRole("Muhely_Szentgyorgy") || User.IsInRole("Iroda_Szentgyorgy"))
            {
                isDone = from s in db.Mains
                         where s.Customer.Date == DateTime.Today && s.isDone == true && s.Sfantu_MCiuc == true
                         orderby s.Customer.Date descending
                         select s;
            }
            if (User.IsInRole("Administrator"))
            {
                isDone = from s in db.Mains
                         where s.Customer.Date == DateTime.Today
                         orderby s.Customer.Date descending
                         select s;
            }

            ViewBag.dimTypeID = new SelectList(from s in db.DimTypes orderby s.Name select s, "dimTypeID", "Name");
            return View(isDone.ToList());
        }

        public ActionResult PJCreate(string dimensiune, int dimTypeID)
        {
            var customerService = new Main();
            PopulateAssignedServiciiData(customerService, dimTypeID);
            ViewBag.AnvelopeID = new SelectList(from s in db.Anvelopes orderby s.Marca select s, "AnvelopeID", "Marca");
            ViewBag.ProfilID = new SelectList(from s in db.Profils orderby s.Name select s, "ProfilID", "Name");
            ViewBag.CarTypeID = new SelectList(from s in db.CarTypes orderby s.Name select s, "CarTypeID", "Name");
            ViewBag.OperatorID = new SelectList(from s in db.Operators orderby s.Name select s, "OperatorID", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult PJCreate(CustomerService model, Main main, CarDetail carDetail, CarType carType, string[] selectedServicii, string[] selectedCantitate, string dimensiune, int dimTypeID)
        {
            if (ModelState.IsValid)
            {
                if (selectedServicii != null)
                {
                    //save for the depo the firstName and the lastName to
                    if (main.Depozitare.IsDepozitare)
                    {
                        main.Depozitare.FirstName = main.Customer.Client;
                        main.Depozitare.LastName = main.Customer.Delegat;
                        main.Depozitare.NrInmatricuare = main.CarDetail.NrInmatricular;
                    }
                    if (User.IsInRole("Muhely_Szentgyorgy") || User.IsInRole("Iroda_Szentgyorgy"))
                    {
                        main.Sfantu_MCiuc = true;
                        main.Depozitare.Sfantu_Mciuc = true;
                    }
                    if (main.OperatorID != null)
                    {
                        var operator_name = db.Operators.Find(main.OperatorID);
                        main.OperatorName = operator_name.Name;
                    }
                    dimType rimToFind = db.DimTypes.Find(dimTypeID);
                    main.CarDetail.Rim = rimToFind.Name;
                    main.CarDetail.DimensiuneaA = dimensiune;
                    main.Customer.pFizica_pJuridica = false;
                    db.Mains.Add(main);

                    var i = 0;
                    foreach (var servicii in selectedServicii)
                    {
                        var serviciiToAdd = db.PangusServiciis.Find(int.Parse(servicii));
                        model.NamedServ = serviciiToAdd.Name;
                        model.Pret = serviciiToAdd.Price;
                        model.IsEditable = serviciiToAdd.IsEditable;
                        while (selectedCantitate[i] == "0")
                        {
                            i++;
                        }
                        model.Cantitate = float.Parse(selectedCantitate[i]);
                        i++;
                        db.CustomerServices.Add(model);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }

            }
            PopulateAssignedServiciiData(main, dimTypeID);
            ViewBag.AnvelopeID = new SelectList(from s in db.Anvelopes orderby s.Marca select s, "AnvelopeID", "Marca");
            ViewBag.ProfilID = new SelectList(from s in db.Profils orderby s.Name select s, "ProfilID", "Name");
            ViewBag.CarTypeID = new SelectList(from s in db.CarTypes orderby s.Name select s, "CarTypeID", "Name");
            ViewBag.OperatorID = new SelectList(from s in db.Operators orderby s.Name select s, "OperatorID", "Name");
            return View(main);
        }

        public ActionResult Create(string dimensiune, int dimTypeID)
        {
            var customerService = new Main();
            PopulateAssignedServiciiData(customerService, dimTypeID);
            ViewBag.AnvelopeID = new SelectList(from s in db.Anvelopes orderby s.Marca select s, "AnvelopeID", "Marca");
            ViewBag.ProfilID = new SelectList(from s in db.Profils orderby s.Name select s, "ProfilID", "Name");
            ViewBag.CarTypeID = new SelectList(from s in db.CarTypes orderby s.Name select s, "CarTypeID", "Name");
            ViewBag.OperatorID = new SelectList(from s in db.Operators orderby s.Name select s, "OperatorID", "Name");
            return View();
        }

        public JsonResult SelectProfils(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            IEnumerable<Profil> profils = db.Profils.Where(x => x.AnvelopeID == id);
            profils = from s in profils orderby s.Name select s;
            return Json(profils);
        }

        public JsonResult SelectFirmes(string term)
        {
            List<string> firmes;
            firmes = db.Firmes.Where(x => x.Name.StartsWith(term)).Select(y => y.Name).ToList();
            return Json(firmes, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(CustomerService model, Main main, CarDetail carDetail, CarType carType, string[] selectedServicii, string[] selectedCantitate, string dimensiune, int dimTypeID)
        {
            if (ModelState.IsValid)
            {
                
                if (selectedServicii != null)
                {
                    //save for the depo the firstName and the lastName to
                    if (main.Depozitare.IsDepozitare)
                    {
                        main.Depozitare.FirstName = main.Customer.FirstName;
                        main.Depozitare.LastName = main.Customer.LastName;
                        main.Depozitare.NrInmatricuare = main.CarDetail.NrInmatricular;
                    }

                    if (User.IsInRole("Muhely_Szentgyorgy") || User.IsInRole("Iroda_Szentgyorgy"))
                    {
                        main.Sfantu_MCiuc = true;
                        main.Depozitare.Sfantu_Mciuc = true;
                    }
                    if (main.OperatorID != null)
                    {
                        var operator_name = db.Operators.Find(main.OperatorID);
                        main.OperatorName = operator_name.Name;
                    }
                    dimType rimToFind = db.DimTypes.Find(dimTypeID);
                    main.CarDetail.Rim = rimToFind.Name;
                    main.CarDetail.DimensiuneaA = dimensiune;
                    main.Customer.pFizica_pJuridica = true;
                    db.Mains.Add(main);

                    var i = 0;
                    foreach (var servicii in selectedServicii)
                    {
                        var serviciiToAdd = db.PangusServiciis.Find(int.Parse(servicii));
                        model.NamedServ = serviciiToAdd.Name;
                        model.Pret = serviciiToAdd.Price;
                        model.IsEditable = serviciiToAdd.IsEditable;
                        while (selectedCantitate[i] == "0")
                        {
                            i++;
                        }
                        model.Cantitate = float.Parse(selectedCantitate[i]);
                        i++;
                        db.CustomerServices.Add(model);
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }
            }
            PopulateAssignedServiciiData(main, dimTypeID);
            ViewBag.AnvelopeID = new SelectList(from s in db.Anvelopes orderby s.Marca select s, "AnvelopeID", "Marca");
            ViewBag.ProfilID = new SelectList(from s in db.Profils orderby s.Name select s, "ProfilID", "Name");
            ViewBag.CarTypeID = new SelectList(from s in db.CarTypes orderby s.Name select s, "CarTypeID", "Name");
            ViewBag.OperatorID = new SelectList(from s in db.Operators orderby s.Name select s, "OperatorID", "Name");
            return View(main);
        }

        private void PopulateAssignedServiciiData(Main customer, int dimTypeID)
        {
            //var allServices = db.PangusServiciis;
            var allServices = from s in db.PangusServiciis
                              where s.dimTypeID == dimTypeID
                              orderby s.Name
                              select s;

            var viewModel = new List<AssignedServicesData>();
            foreach (var service in allServices)
            {
                viewModel.Add(new AssignedServicesData
                {
                    ServiciiId = service.ID,
                    ServiciiName = service.Name,
                    Price = service.Price
                });
            }
            ViewBag.Serviciis = viewModel;
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

    }
}