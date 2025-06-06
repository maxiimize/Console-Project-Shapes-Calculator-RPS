using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Strategies
{
    public class MultiplyStrategy : ICalculationStrategy
    {
        public string Operator => "*";

        public double Calculate(double operand1, double? operand2)
        {
            if (!operand2.HasValue)
                throw new InvalidOperationException("Multiplikation kräver två operandvärden.");

            return Math.Round(operand1 * operand2.Value, 2);
        }
    }
}