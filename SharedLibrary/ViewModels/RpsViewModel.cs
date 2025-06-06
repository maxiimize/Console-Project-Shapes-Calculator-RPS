using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.ViewModels
{
    public class RpsViewModel
    {
        public int Id { get; set; }
        public string PlayerMove { get; set; }
        public string ComputerMove { get; set; }
        public string Outcome { get; set; }
        public decimal WinRate { get; set; }
        public string DatePlayed { get; set; }
    }
}
