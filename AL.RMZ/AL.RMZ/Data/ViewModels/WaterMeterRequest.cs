using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Models
{
    public class WaterMeterRequest
    {
        [Required]
        public string Number { get; set; }

        [Required]
        public int ZoneId { get; set; }

    }
}
