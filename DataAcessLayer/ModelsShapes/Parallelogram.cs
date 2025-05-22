using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.ModelsShapes
{
    public class Parallelogram : Shape
    {
        [Required]
        public double BaseLength { get; set; }

        [Required]
        public double SideLength { get; set; }

        [Required]
        public double Height { get; set; }
    }
}
