using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PangusServices.Models
{
    public class Main
    {
        public int MainID { get; set; }

        public int CustomerID { get; set; }
        public int? PaymentMethodID { get; set; }
        public int CarDetailID { get; set; }
        public int? AnvelopeNoiID { get; set; }
        public int? DepozitareID { get; set; }
        public bool isDone { get; set; }
        public float? Discount { get; set; }
        public bool Sfantu_MCiuc {get; set;}
        public string OperatorName { get; set; }
        public int? OperatorID { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public virtual CarDetail CarDetail { get; set; }
        public virtual AnvelopeNoi AnvelopeNoi { get; set; }
        public virtual Depozitare Depozitare { get; set; }
        public virtual Operator Operator { get; set; }

        public virtual ICollection<CustomerService> CustomerServices { get; set; }

    }
}