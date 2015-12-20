using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PangusServices.Models
{
    public class Firme
    {
        public int FirmeID { get; set; }
        public string Name { get; set; }
        public string Ro { get; set; }
        public string CUI { get; set; }
        public string Adresa { get; set; }
    }
}