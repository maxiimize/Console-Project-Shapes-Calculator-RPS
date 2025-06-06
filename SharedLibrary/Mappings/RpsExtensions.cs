using DataAcessLayer.ModelsRPS;
using SharedLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Mappings
{
    public static class RpsExtensions
    {
        public static RpsViewModel ToViewModel(this RPS r)
        {
            return new RpsViewModel
            {
                Id = r.Id,
                PlayerMove = r.PlayerMove,
                ComputerMove = r.ComputerMove,
                Outcome = r.Outcome,
                WinRate = r.WinRate,
                DatePlayed = r.DatePlayed.ToString("yyyy-MM-dd HH:mm")
            };
        }
    }
}
