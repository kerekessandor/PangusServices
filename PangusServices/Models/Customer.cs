using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PangusServices.Models
{
    public class Customer
    {
        public int ID { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Email { get; set; }

        public int? PhoneNumber { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }
        public bool pFizica_pJuridica { get; set; }
        public string Client { get; set; }
        public string Delegat { get; set; }

        public virtual ICollection<Main> Mains { get; set; }
        

    }
}