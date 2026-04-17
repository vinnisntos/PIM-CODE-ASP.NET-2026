using System.ComponentModel.DataAnnotations;

namespace PIM2026.Models
{
    public class Agendamento
    {
        [Key]
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public int ServicoId { get; set; }

        public DateTime DataHora { get; set; }

        public string Status { get; set; } = "Pendente";

        public virtual Usuario Cliente { get; set; } = null!;

        public virtual Servico Servico { get; set; } = null!;
    }
}