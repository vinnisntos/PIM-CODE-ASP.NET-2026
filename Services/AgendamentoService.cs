using PIM2026.Data;
using PIM2026.Models;

namespace PIM2026.Services
{
    public class AgendamentoService
    {
        private readonly AppDbContext _context;

        public AgendamentoService(AppDbContext context)
        {
            _context = context;
        }

        public (bool Sucesso, string Mensagem, Agendamento? Agendamento) CriarAgendamento(int usuarioId, int servicoId, DateTime dataHora, string observacoes = "")
        {
            var servico = _context.Servicos.Find(servicoId);
            if (servico == null)
            {
                return (false, "Serviço não encontrado.", null);
            }

            var hora = dataHora.TimeOfDay;
            if (hora < TimeSpan.FromHours(9) || hora > TimeSpan.FromHours(18))
            {
                return (false, "Horário fora do expediente. Funcionamos das 9h às 18h.", null);
            }

            if (dataHora.Date < DateTime.Today)
            {
                return (false, "Não é possível agendar para datas passadas.", null);
            }

            var diferenca = dataHora - DateTime.Now;
            if (diferenca.TotalHours < 24)
            {
                return (false, "O agendamento deve ser feito com pelo menos 24 horas de antecedência.", null);
            }

            if (dataHora.DayOfWeek == DayOfWeek.Sunday)
            {
                return (false, "Não atendemos aos domingos.", null);
            }

            var dataHoraFim = dataHora.AddMinutes(servico.DuracaoMinutos);

            var conflito = _context.Agendamentos
                .Where(a => a.DataHora.Date == dataHora.Date && a.Status != "Cancelado")
                .Any(a =>
                     (dataHora >= a.DataHora && dataHora < a.DataHoraFim) ||
                     (dataHoraFim > a.DataHora && dataHoraFim <= a.DataHoraFim) ||
                     (dataHora <= a.DataHora && dataHoraFim >= a.DataHoraFim)
                  );

            if (conflito)
            {
                return (false, "Já existe um agendamento neste horário. Por favor, escolha outro horário.", null);
            }

            var codigoConfirmacao = GerarCodigoConfirmacao();

            var agendamento = new Agendamento
            {
                UsuarioId = usuarioId,
                ServicoId = servicoId,
                DataHora = dataHora,
                DataHoraFim = dataHoraFim,
                Status = "Pendente",
                Observacoes = observacoes,
                CodigoConfirmacao = codigoConfirmacao
            };

            _context.Agendamentos.Add(agendamento);
            _context.SaveChanges();

            return (true, $"Agendamento criado com sucesso! Seu código é: {codigoConfirmacao}", agendamento);
        }

        public (bool Sucesso, string Mensagem) CancelarAgendamento(int agendamentoId, int usuarioId, string? motivo = null)
        {
            var agendamento = _context.Agendamentos.Find(agendamentoId);

            if (agendamento == null)
            {
                return (false, "Agendamento não encontrado.");
            }

            if (agendamento.UsuarioId != usuarioId)
            {
                return (false, "Você não tem permissão para cancelar este agendamento.");
            }

            if (agendamento.Status == "Cancelado")
            {
                return (false, "Este agendamento já está cancelado.");
            }

            if (agendamento.Status == "Concluído")
            {
                return (false, "Não é possível cancelar um agendamento já concluído.");
            }

            var horasAteAgendamento = (agendamento.DataHora - DateTime.Now).TotalHours;
            if (horasAteAgendamento < 24)
            {
                return (false, "O cancelamento deve ser feito com pelo menos 24 horas de antecedência.");
            }

            agendamento.Status = "Cancelado";
            agendamento.DataCancelamento = DateTime.Now;
            agendamento.MotivoCancelamento = motivo;
            agendamento.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();

            return (true, "Agendamento cancelado com sucesso.");
        }

        public List<DateTime> ObterHorariosDisponiveis(DateTime data, int servicoId)
        {
            var servico = _context.Servicos.Find(servicoId);
            if (servico == null) return new List<DateTime>();

            var horarios = new List<DateTime>();
            var inicio = data.Date.AddHours(9);
            var fim = data.Date.AddHours(18).AddMinutes(-servico.DuracaoMinutos);

            var agendamentosDoDia = _context.Agendamentos
                .Where(a => a.DataHora.Date == data.Date && a.Status != "Cancelado")
                .Select(a => new { a.DataHora, a.DataHoraFim })
                .ToList();

            for (var horario = inicio; horario <= fim; horario = horario.AddMinutes(30))
            {
                var horarioFim = horario.AddMinutes(servico.DuracaoMinutos);

                var conflito = agendamentosDoDia.Any(a =>
                    (horario >= a.DataHora && horario < a.DataHoraFim) ||
                    (horarioFim > a.DataHora && horarioFim <= a.DataHoraFim) ||
                    (horario <= a.DataHora && horarioFim >= a.DataHoraFim)
                );

                if (!conflito)
                {
                    horarios.Add(horario);
                }
            }

            return horarios;
        }

        private string GerarCodigoConfirmacao()
        {
            return "SB-" + Guid.NewGuid().ToString("N")[..6].ToUpper();
        }
    }
}
