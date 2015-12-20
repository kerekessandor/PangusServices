using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PangusServices.Models
{
    public class PangusServicii
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public bool IsEditable { get; set; }
        public int dimTypeID {get;set;}
        public virtual dimType dimType { get; set; }
        public virtual ICollection<CustomerService> CustomerServices { get; set; }
    }
}