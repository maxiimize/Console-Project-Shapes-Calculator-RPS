using DataAcessLayer.ModelsShapes;
using SharedLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Mappings
{
    public static class ShapeExtensions
    {
        public static ShapeViewModel ToViewModel(this Shape s)
        {
            string parameters = s switch
            {
                Rectangle r => $"W={r.Width}, H={r.Height}",
                Parallelogram p => $"B={p.BaseLength}, S={p.SideLength}, H={p.Height}",
                Triangle t => $"Base={t.BaseLength}, H={t.Height}, S1={t.SideA}, S2={t.SideB}",
                Rhombus h => $"S={h.SideLength}, H={h.Height}",
                _ => string.Empty
            };
            return new ShapeViewModel
            {
                Id = s.Id,
                ShapeType = s.GetType().Name,
                Parameters = parameters,
                Area = s.Area,
                Perimeter = s.Perimeter,
                DateCreated = s.DateCreated.ToString("yyyy-MM-dd HH:mm")
            };
        }
    }
}
