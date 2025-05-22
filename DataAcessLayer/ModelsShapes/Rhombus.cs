using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace DataAcessLayer.ModelsShapes
{
    public class Rhombus : Shape
    {
        [Required]
        public double SideLength { get; set; }

        [Required]
        public double Diagonal1 { get; set; }

        [Required]
        public double Diagonal2 { get; set; }
    }
}
