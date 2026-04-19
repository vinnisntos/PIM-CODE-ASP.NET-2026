using System.ComponentModel.DataAnnotations;

namespace PIM2026.Models
{
    public class Servico
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do serviço é obrigatório")]
        public string Nome { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "A duração é obrigatória")]
        [Range(1, 480, ErrorMessage = "A duração deve ser entre 1 e 480 minutos")]
        public int DuracaoMinutos { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório")]
        [Range(0.01, 99999.99, ErrorMessage = "O preço deve ser maior que zero")]
        public decimal Preco { get; set; }

        public string CorHex { get; set; } = "#FF6B6B";

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
    }
}
