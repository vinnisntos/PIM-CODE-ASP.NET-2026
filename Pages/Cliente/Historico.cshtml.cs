using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PIM2026.Data;
using PIM2026.Models;
using PIM2026.Services;

namespace PIM2026.Pages.Cliente
{
    public class HistoricoModel : PageModel
    {
        private readonly AppDbContext _context;

        public HistoricoModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Agendamento> AgendamentosFuturos { get; set; } = new();
        public List<Agendamento> AgendamentosPassados { get; set; } = new();

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

            var usuarioId = AuthHelper.GetUsuarioId(HttpContext)!.Value;
            var hoje = DateTime.Today;

            AgendamentosFuturos = _context.Agendamentos
                .Where(a => a.UsuarioId == usuarioId && a.DataHora.Date >= hoje && a.Status != "Cancelado")
                .Include(a => a.Servico)
                .OrderBy(a => a.DataHora)
                .ToList();

            AgendamentosPassados = _context.Agendamentos
                .Where(a => a.UsuarioId == usuarioId && (a.DataHora.Date < hoje || a.Status == "Cancelado" || a.Status == "Concluído"))
                .Include(a => a.Servico)
                .OrderByDescending(a => a.DataHora)
                .Take(20)
                .ToList();

            return Page();
        }

        public IActionResult OnPostCancelar(int id)
        {
            if (!AuthHelper.IsAuthenticated(HttpContext) || !AuthHelper.IsCliente(HttpContext))
            {
                return RedirectToPage("/Index");
            }

            var usuarioId = AuthHelper.GetUsuarioId(HttpContext)!.Value;
            var agendamento = _context.Agendamentos
                .FirstOrDefault(a => a.Id == id && a.UsuarioId == usuarioId);

            if (agendamento != null)
            {
                var horasAteAgendamento = (agendamento.DataHora - DateTime.Now).TotalHours;
                if (horasAteAgendamento >= 24)
                {
                    agendamento.Status = "Cancelado";
                    agendamento.DataCancelamento = DateTime.Now;
                    agendamento.UpdatedAt = DateTime.UtcNow;
                    _context.SaveChanges();
                }
            }

            return RedirectToPage();
        }
    }
}
