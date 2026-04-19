using System.ComponentModel.DataAnnotations;

namespace PIM2026.Models
{
    public class Agendamento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int ServicoId { get; set; }

        [Required(ErrorMessage = "A data e hora são obrigatórias")]
        public DateTime DataHora { get; set; }

        public DateTime DataHoraFim { get; set; }

        public string Status { get; set; } = "Pendente";

        public string Observacoes { get; set; } = string.Empty;

        public string? CodigoConfirmacao { get; set; }

        public DateTime? DataCancelamento { get; set; }

        public string? MotivoCancelamento { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual Usuario Cliente { get; set; } = null!;

        public virtual Servico Servico { get; set; } = null!;
    }
}
