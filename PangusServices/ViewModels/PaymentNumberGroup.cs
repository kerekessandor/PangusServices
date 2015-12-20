using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PangusServices.Models;

namespace PangusServices.ViewModels
{
    public class PaymentNumberGroup
    {
        
        public string PaymentMethod { get; set; }

        public int PaymentCount { get; set; }
    }
}