using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Strategies
{
    public class ModulusStrategy : ICalculationStrategy
    {
        public string Operator => "%";

        public double Calculate(double operand1, double? operand2)
        {
            if (!operand2.HasValue)
                throw new InvalidOperationException("Modulus (resten) kräver två operandvärden.");

            if (operand2.Value == 0)
                throw new DivideByZeroException("Division med noll (modulus) är inte tillåtet.");

            return Math.Round(operand1 % operand2.Value, 2);
        }
    }
}
