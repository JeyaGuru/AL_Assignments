using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Models
{
    public class WaterMeterDetailRequest
    {
        public int WaterMeterId { get; set; }
        public DateTime ReadingDate { get; set; }

        public int StartReading { get; set; }

        public int EndReading { get; set; }     
    }
}
