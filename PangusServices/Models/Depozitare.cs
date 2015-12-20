using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PangusServices.Models
{
    public class Depozitare
    {
        public int DepozitareID { get; set; }

        [Required]
        public bool IsDepozitare { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NrInmatricuare { get; set; }
        public string Dimensiune { get; set; }
        public int? Cantitate { get; set; }
        public string Descriere { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Data { get; set; }
        public int? ProfilID { get; set; }
        public bool Sfantu_Mciuc { get; set; }

        public virtual Profil Profil { get; set; }
    }
}