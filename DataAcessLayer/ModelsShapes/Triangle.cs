using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace DataAcessLayer.ModelsShapes
{
    public class Triangle : Shape
    {
        [Required]
        public double SideA { get; set; }

        [Required]
        public double SideB { get; set; }

        [Required]
        public double SideC { get; set; }
    }
}
