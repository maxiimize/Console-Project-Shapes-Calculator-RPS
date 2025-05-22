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
        public double Diagonal1 { get; set; }
        public double Diagonal2 { get; set; }
        public override void Calculate()
        {
            Area = (Diagonal1 * Diagonal2) / 2;
            Perimeter = 4 * SideLength;
        }
    }
}
