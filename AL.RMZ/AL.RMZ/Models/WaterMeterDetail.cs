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

        public int WaterMeterId { get; set; }

        [ForeignKey("WaterMeterId")]
        public WaterMeter WaterMeter { get; set; }

        public DateTime ReadingDate { get; set; }

        public int StartReading { get; set; }

        public int EndReading { get; set; }

        public int TotalUnits { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }

        public int CreatedById { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int UpdatedById { get; set; }
    }
}
