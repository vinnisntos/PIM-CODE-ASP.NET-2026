using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PIM2026.Data;
using PIM2026.Models;
using PIM2026.Services;

namespace PIM2026.Pages.Profissional
{
    public class DashboardModel : PageModel
    {
        private readonly AppDbContext _context;

        public DashboardModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Agendamento> AgendamentosHoje { get; set; } = new();
        public List<Agendamento> AgendamentosProximos { get; set; } = new();
        public List<Agendamento> AgendamentosPendentes { get; set; } = new();
        public int TotalClientes { get; set; }
        public int TotalServicos { get; set; }
        public int AgendamentosMes { get; set; }

        public IActionResult OnGet()
        {
            if (!AuthHelper.IsAuthenticated(HttpContext))
            {
                return RedirectToPage("/Index");
            }

            if (!AuthHelper.IsProfissional(HttpContext))
            {
                return RedirectToPage("/Cliente/Agendar");
            }

            CarregarDashboard();
            return Page();
        }

        public IActionResult OnPostConfirmar(int id)
        {
            if (!AuthHelper.IsProfissional(HttpContext))
            {
                return RedirectToPage("/Cliente/Agendar");
            }

            var agendamento = _context.Agendamentos.Find(id);
            if (agendamento != null)
            {
                agendamento.Status = "Confirmado";
                agendamento.UpdatedAt = DateTime.UtcNow;
                _context.SaveChanges();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostCancelar(int id)
        {
            if (!AuthHelper.IsProfissional(HttpContext))
            {
                return RedirectToPage("/Cliente/Agendar");
            }

            var agendamento = _context.Agendamentos.Find(id);
            if (agendamento != null)
            {
                agendamento.Status = "Cancelado";
                // Corrigido: Era DateTime.Now, agora é UtcNow pra não crashar o Postgres
                agendamento.DataCancelamento = DateTime.UtcNow;
                agendamento.UpdatedAt = DateTime.UtcNow;
                _context.SaveChanges();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostConcluir(int id)
        {
            if (!AuthHelper.IsProfissional(HttpContext))
            {
                return RedirectToPage("/Cliente/Agendar");
            }

            var agendamento = _context.Agendamentos.Find(id);
            if (agendamento != null)
            {
                agendamento.Status = "Concluído";
                agendamento.UpdatedAt = DateTime.UtcNow;
                _context.SaveChanges();
            }

            return RedirectToPage();
        }

        private void CarregarDashboard()
        {
            // O Carimbo UTC perfeito
            var hoje = DateTime.UtcNow.Date.ToUniversalTime();
            var amanha = hoje.AddDays(1);
            var proximaSemana = hoje.AddDays(7);
            var limiteProximaSemana = proximaSemana.AddDays(1);

            AgendamentosHoje = _context.Agendamentos
                // Substituído o .Date == hoje por busca em range (Muito mais rápido e não buga o Postgres)
                .Where(a => a.DataHora >= hoje && a.DataHora < amanha && a.Status != "Cancelado")
                .Include(a => a.Cliente)
                .Include(a => a.Servico)
                .OrderBy(a => a.DataHora)
                .ToList();

            AgendamentosProximos = _context.Agendamentos
                // Mesma coisa aqui, usando range pra ficar liso
                .Where(a => a.DataHora >= amanha && a.DataHora < limiteProximaSemana && a.Status != "Cancelado")
                .Include(a => a.Cliente)
                .Include(a => a.Servico)
                .OrderBy(a => a.DataHora)
                .Take(10)
                .ToList();

            AgendamentosPendentes = _context.Agendamentos
                .Where(a => a.Status == "Pendente" && a.DataHora >= hoje)
                .Include(a => a.Cliente)
                .Include(a => a.Servico)
                .OrderBy(a => a.DataHora)
                .ToList();

            TotalClientes = _context.Usuarios.Count(u => u.Perfil == "Cliente");
            TotalServicos = _context.Servicos.Count(s => s.IsActive);
            AgendamentosMes = _context.Agendamentos
                .Count(a => a.DataHora.Month == hoje.Month && a.DataHora.Year == hoje.Year && a.Status == "Concluído");
        }
    }
}