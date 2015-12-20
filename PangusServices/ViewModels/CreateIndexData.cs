using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PangusServices.Models;

namespace PangusServices.ViewModels
{
    public class CreateIndexData
    {
        public IEnumerable<Main> Mains { get; set; }
        public IEnumerable<Customer> Customers { get; set; }
        public IEnumerable<PangusServicii> PangusServiciis { get; set; }
        public IEnumerable<CustomerService> CustomerServices { get; set; }
        public IEnumerable<Profil> Profils { get; set; }
        public IEnumerable<CarDetail> CarDetails { get; set; }
        public IEnumerable<Depozitare> Depozitare { get; set; }
        public IEnumerable<PaymentMethod> PaymentMethod { get; set; }

    }
}