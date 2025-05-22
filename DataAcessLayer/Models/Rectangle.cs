using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Models
{
    public class Rectangle : Shape
    {
        [Required]
        public double Width { get; set; }

        [Required]
        public double Height { get; set; }
    }
}
