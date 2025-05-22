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
        public double SideA { get; set; }
        public double SideB { get; set; }
        public double SideC { get; set; }
        public override void Calculate()
        {
            var s = (SideA + SideB + SideC) / 2;
            Area = Math.Sqrt(s * (s - SideA) * (s - SideB) * (s - SideC));
            Perimeter = SideA + SideB + SideC;
        }
    }
}
