using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PangusServices.Models
{
    public class CarDetail
    {
        public int CarDetailID { get; set; }

        [Required]
        public string NrInmatricular { get; set; }

        public string MarcaAuto { get; set; }

        public float? KmRulati { get; set; }

        public string DimensiuneaA { get; set; }
        public string Rim { get; set; }

        [Required(ErrorMessage="ALUMINIU sau OTEL")]
        public bool AluminiuOtel { get; set; }

        public string Descriere { get; set; }

        public int CarTypeID { get; set; }

        public virtual CarType CarType { get; set; }

    }
}