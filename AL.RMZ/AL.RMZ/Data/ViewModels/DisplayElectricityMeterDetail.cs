using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Models
{
    public class DisplayElectricityMeterDetail
    {
        public int id { get; set; }
        public string facilityname { get; set; }

        public string buildingname { get; set; }

        public string zonename { get; set; }
        public string electricitymeter { get; set; }

        public DateTime readingdate { get; set; }

        public int startunit { get; set; }

        public int endunit { get; set; }

        public int totalunits { get; set; }

    }
}
