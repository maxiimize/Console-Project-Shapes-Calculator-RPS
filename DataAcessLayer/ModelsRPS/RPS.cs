using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAcessLayer.ModelsRPS
{
    public class RPS
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string PlayerMove { get; set; }

        [Required]
        public string ComputerMove { get; set; }

        [Required]
        public string Outcome { get; set; }

        [Required]
        public DateTime DatePlayed { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal WinRate { get; set; }
    }
}
