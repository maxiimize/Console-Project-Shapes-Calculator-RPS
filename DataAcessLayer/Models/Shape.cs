using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Models
{
    public abstract class Shape
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public double Area { get; set; }

        [Required]
        public double Perimeter { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }
    }
}
