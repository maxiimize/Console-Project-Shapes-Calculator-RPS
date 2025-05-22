using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace DataAcessLayer.ModelsShapes
{
    public abstract class Shape
    {
        public int Id { get; set; }
        public double Area { get; protected set; }
        public double Perimeter { get; protected set; }
        public DateTime DateCreated { get; set; }
        public abstract void Calculate();
    }
}
