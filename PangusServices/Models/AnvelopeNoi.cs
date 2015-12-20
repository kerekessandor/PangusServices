using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PangusServices.Models
{
    public class AnvelopeNoi
    {
        public int AnvelopeNoiID { get; set; }
        public bool NoiRulate { get; set; }
        public float? DotSerie { get; set; }
        public int? Cantitate { get; set; }
        public int? ProfilID { get; set; }
        public string ProfilName { get; set; }
        public string AnvelopeName { get; set; }

        public virtual Profil Profil { get; set; }
    }
}