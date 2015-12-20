using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PangusServices.ViewModels
{
    public class AssignedServicesData
    {
        public int ServiciiId { get; set; }
        public string ServiciiName { get; set; }
        public float Price { get; set; }
        public bool Assigned { get; set; }
    }
}