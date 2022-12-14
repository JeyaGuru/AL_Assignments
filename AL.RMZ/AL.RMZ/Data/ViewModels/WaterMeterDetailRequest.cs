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
        [Required]
        public int WaterMeterId { get; set; }

        [Required]
        public DateTime ReadingDate { get; set; }

        [Required]
        public int StartReading { get; set; }

        [Required]
        public int EndReading { get; set; }
    }
}
