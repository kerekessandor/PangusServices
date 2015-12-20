using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PangusServices.Models
{
    public class dimType
    {
        public int dimTypeID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PangusServicii> PangusServiciis { get; set; }
    }
}