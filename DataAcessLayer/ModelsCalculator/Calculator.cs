using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.ModelsCalculator
{
    public class Calculator
    {
        public int Id { get; set; }
        public double Operand1 { get; set; }
        public double? Operand2 { get; set; }    
        public string Operator { get; set; }     
        public double Result { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
