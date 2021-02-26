using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Lab2_DiegoRamirez_DanielElias.Models
{
    public class Drug
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Factory { get; set; }

        [Required]
        public string Price { get; set; }

        [Required]
        public int Stock { get; set; }

    }
}
