using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PangusServices.Models
{

    public class Operator
    {
        public int OperatorID { get; set; }
        public string Name { get; set; }
        public int? PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}