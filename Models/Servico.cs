using System.ComponentModel.DataAnnotations;

namespace PIM2026.Models
{
    public class Servico
    {
        [Key]
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public int DuracaoMinutos { get; set; }

        public decimal Preco { get; set; }
    }
}