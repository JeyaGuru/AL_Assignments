using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Models
{
    public class WaterMeter
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public int ZoneId { get; set; }

        [ForeignKey("ZoneId")]
        public Zone Zone { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedById { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? UpdatedById { get; set; }

        public WaterMeter()
        {
            CreatedDate = DateTime.Now;
        }

    }
}
