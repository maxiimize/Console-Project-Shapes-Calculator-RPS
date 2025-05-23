using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace DataAcessLayer.ModelsShapes
{
    public class Parallelogram : Shape
    {
        public double BaseLength { get; set; }
        public double SideLength { get; set; }
        public double Height { get; set; }
        public override void Calculate()
        {
            Area = BaseLength * Height;
            Perimeter = 2 * (BaseLength + SideLength);
        }
    }
}
