using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Strategies
{
    public class SqrtStrategy : ICalculationStrategy
    {
        public string Operator => "√";

        public double Calculate(double operand1, double? operand2)
        {
            if (operand1 < 0)
                throw new InvalidOperationException("Kan inte ta kvadratrot av ett negativt tal.");

            return Math.Round(Math.Sqrt(operand1), 2);
        }
    }
}
