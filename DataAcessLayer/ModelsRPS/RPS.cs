using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.ModelsRPS
{
    public class RPS
    {
        public int Id { get; set; }
        public string PlayerMove { get; set; }   
        public string ComputerMove { get; set; }
        public string Outcome { get; set; }      
        public DateTime DatePlayed { get; set; }
    }
}
