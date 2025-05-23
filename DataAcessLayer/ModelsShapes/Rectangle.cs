using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace DataAcessLayer.ModelsShapes
{
    public class Rectangle : Shape
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public override void Calculate()
        {
            Area = Width * Height;
            Perimeter = 2 * (Width + Height);
        }
    }
}
