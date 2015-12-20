using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PangusServices.Models
{
    public class Anvelope
    {
        public int AnvelopeID { get; set; }

        public string Marca { get; set; }

        public virtual ICollection<Profil> Profils { get;set; }
    }
}