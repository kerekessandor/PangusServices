using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PangusServices.Models
{
    public class CustomerService
    {
        public int ID { get; set; }
        public float Cantitate { get; set; }
        public string NamedServ { get; set; }
        public float Pret { get; set; }
        public string Dimensiune { get; set; }
        public bool IsEditable { get; set; }
        public int MainID { get; set; }
 
        public virtual Main Main { get; set; }



    }
}