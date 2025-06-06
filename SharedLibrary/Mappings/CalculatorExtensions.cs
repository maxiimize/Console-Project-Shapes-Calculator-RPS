using DataAcessLayer.ModelsCalculator;
using SharedLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Mappings
{
    public static class CalculatorExtensions
    {
        public static CalculatorViewModel ToViewModel(this Calculator c)
        {
            return new CalculatorViewModel
            {
                Id = c.Id,
                Operand1 = c.Operand1,
                Operand2 = c.Operand2,
                Operator = c.Operator,
                Result = c.Result,
                DateCreated = c.DateCreated.ToString("yyyy-MM-dd HH:mm")
            };
        }
    }
}
