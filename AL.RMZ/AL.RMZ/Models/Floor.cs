using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Models
{
    public class Floor
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public int BuildingId { get; set; }

        [ForeignKey("BuildingId")]
        public Building Building { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedById { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? UpdatedById { get; set; }

        public Floor()
        {
            CreatedDate = DateTime.Now;
        }
    }
}
