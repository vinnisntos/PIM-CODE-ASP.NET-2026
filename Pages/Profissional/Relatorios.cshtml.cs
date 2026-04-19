using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PIM2026.Data;
using PIM2026.Models;
using PIM2026.Services;

namespace PIM2026.Pages.Profissional
{
    public class RelatoriosModel : PageModel
    {
        private readonly AppDbContext _context;

        public RelatoriosModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Agendamento> AgendamentosMes { get; set; } = new();
        public decimal TotalReceitaMes { get; set; }
        public int TotalAtendimentosMes { get; set; }
        public Dictionary<string, int> ServicosMaisAgendados { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int? Mes { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? Ano { get; set; }

        public IActionResult OnGet()
        {
            if (!AuthHelper.IsAuthenticated(HttpContext) || !AuthHelper.IsProfissional(HttpContext))
            {
                return RedirectToPage("/Index");
            }

            Mes ??= DateTime.Now.Month;
            Ano ??= DateTime.Now.Year;

            CarregarRelatorio();
            return Page();
        }

        private void CarregarRelatorio()
        {
            AgendamentosMes = _context.Agendamentos
                .Where(a => a.DataHora.Month == Mes && a.DataHora.Year == Ano && a.Status != "Cancelado")
                .Include(a => a.Cliente)
                .Include(a => a.Servico)
                .OrderBy(a => a.DataHora)
                .ToList();

            TotalReceitaMes = AgendamentosMes
                .Where(a => a.Status == "Concluído")
                .Sum(a => a.Servico.Preco);

            TotalAtendimentosMes = AgendamentosMes.Count(a => a.Status == "Concluído");

            ServicosMaisAgendados = AgendamentosMes
                .GroupBy(a => a.Servico.Nome)
                .ToDictionary(g => g.Key, g => g.Count());
        }
    }
}
