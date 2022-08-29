using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Models
{
    public class FacilityRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int CityId { get; set; }
    }
}
