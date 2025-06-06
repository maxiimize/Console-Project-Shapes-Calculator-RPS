using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Strategies
{
    public interface ICalculationStrategy
    {
      
        string Operator { get; }

        double Calculate(double operand1, double? operand2);
    }
}

