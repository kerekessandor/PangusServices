using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PangusServices.Models
{
    public class Profil
    {
        public int ProfilID { get; set; }

        public string Name { get; set; }

        public int AnvelopeID { get; set; }

        public virtual Anvelope Anvelope { get; set; }
        public virtual ICollection<Depozitare> Depozitares { get; set; }

    }
}