using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.ViewModels
{
    public class ShapeViewModel
    {
        public int Id { get; set; }
        public string ShapeType { get; set; }
        public string Parameters { get; set; }
        public double Area { get; set; }
        public double Perimeter { get; set; }
        public string DateCreated { get; set; }
    }
}
