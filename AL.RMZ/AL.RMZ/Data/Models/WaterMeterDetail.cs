using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Models
{
    public class WaterMeterDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int WaterMeterId { get; set; }

        [ForeignKey("WaterMeterId")]
        public WaterMeter WaterMeter { get; set; }

        [Required]
        public DateTime ReadingDate { get; set; }

        [Required]
        public int StartReading { get; set; }

        [Required]
        public int EndReading { get; set; }

        public int TotalUnits { get { return EndReading - StartReading; } set { } }

        public DateTime CreatedDate { get; set; }

        public int CreatedById { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? UpdatedById { get; set; }

        public WaterMeterDetail()
        {
            CreatedDate = DateTime.Now;
        }
    }
}
