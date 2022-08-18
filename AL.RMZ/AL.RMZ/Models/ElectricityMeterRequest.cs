using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Models
{
    public class ElectricityMeterRequest
    {

        public string Number { get; set; }


        //public DateTime ReadingDate { get; set; }


        //public int StartReading { get; set; }


        //public int EndReading { get; set; }


        //public string CityName { get; set; }

        //public string FacilityName { get; set; }

        //public string BuildingName { get; set; }

        //public string FloorName { get; set; }

        // public string ZoneName { get; set; }

        public int ZoneId { get; set; }
    }
}
