using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PIM2026.Data;
using PIM2026.Models;
using PIM2026.Services;

namespace PIM2026.Pages.Cliente
{
    public class AgendarModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly AgendamentoService _agendamentoService;

        public AgendarModel(AppDbContext context, AgendamentoService agendamentoService)
        {
            _context = context;
            _agendamentoService = agendamentoService;
        }

        public List<Servico> Servicos { get; set; } = new();
        public List<Agendamento> MeusAgendamentos { get; set; } = new();
        public List<DateTime> HorariosDisponiveis { get; set; } = new();

        [BindProperty]
        public int ServicoId { get; set; }

        [BindProperty]
        public DateTime Data { get; set; } = DateTime.Today.AddDays(1);

        [BindProperty]
        public string? HorarioSelecionado { get; set; }

        [BindProperty]
        public string? Observacoes { get; set; }

        public string? MensagemSucesso { get; set; }
        public string? MensagemErro { get; set; }

        public IActionResult OnGet()
        {
            if (!AuthHelper.IsAuthenticated(HttpContext))
            {
                return RedirectToPage("/Index");
            }

            if (!AuthHelper.IsCliente(HttpContext))
            {
                return RedirectToPage("/Profissional/Dashboard");
            }

            CarregarDados();
            return Page();
        }

        public IActionResult OnPostBuscarHorarios(int servicoId, DateTime data)
        {
            if (!AuthHelper.IsAuthenticated(HttpContext))
            {
                return RedirectToPage("/Index");
            }

            ServicoId = servicoId;
            Data = data;
            HorariosDisponiveis = _agendamentoService.ObterHorariosDisponiveis(data, servicoId);
            CarregarDados();

            return Page();
        }

        public IActionResult OnPostAgendar()
        {
            if (!AuthHelper.IsAuthenticated(HttpContext))
            {
                return RedirectToPage("/Index");
            }

            if (string.IsNullOrEmpty(HorarioSelecionado))
            {
                MensagemErro = "Selecione um horário.";
                CarregarDados();
                return Page();
            }

            var usuarioId = AuthHelper.GetUsuarioId(HttpContext)!.Value;
            var dataHora = DateTime.Parse(HorarioSelecionado);

            var resultado = _agendamentoService.CriarAgendamento(usuarioId, ServicoId, dataHora, Observacoes ?? "");

            if (resultado.Sucesso)
            {
                return RedirectToPage("/Cliente/Sucesso", new { codigo = resultado.Agendamento?.CodigoConfirmacao });
            }
            else
            {
                MensagemErro = resultado.Mensagem;
                CarregarDados();
                return Page();
            }
        }

        private void CarregarDados()
        {
            Servicos = _context.Servicos.Where(s => s.IsActive).ToList();

            var usuarioId = AuthHelper.GetUsuarioId(HttpContext);
            if (usuarioId.HasValue)
            {
                MeusAgendamentos = _context.Agendamentos
                    .Where(a => a.UsuarioId == usuarioId.Value && a.DataHora >= DateTime.Today.ToUniversalTime())
                    .Include(a => a.Servico)
                    .OrderBy(a => a.DataHora)
                    .ToList();
            }

            if (ServicoId > 0)
            {
                HorariosDisponiveis = _agendamentoService.ObterHorariosDisponiveis(Data, ServicoId);
            }
        }
    }
}
