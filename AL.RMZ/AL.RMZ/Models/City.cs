using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }

        public int CreatedById { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int UpdatedById { get; set; }
    }
}
