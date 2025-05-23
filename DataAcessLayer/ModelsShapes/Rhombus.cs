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
        public double SideLength { get; set; }
        public double Height { get; set; }

        public override void Calculate()
        {
            Area = SideLength * Height;
            Perimeter = 4 * SideLength;
        }
    }
}
