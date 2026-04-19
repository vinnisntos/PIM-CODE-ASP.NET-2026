using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PIM2026.Data;
using PIM2026.Services;

namespace PIM2026.Pages.Cliente
{
    public class SucessoModel : PageModel
    {
        private readonly AppDbContext _context;

        public SucessoModel(AppDbContext context)
        {
            _context = context;
        }

        public string CodigoConfirmacao { get; set; } = string.Empty;
        public string NomeServico { get; set; } = string.Empty;
        public DateTime DataHora { get; set; }

        public IActionResult OnGet(string? codigo)
        {
            if (!AuthHelper.IsAuthenticated(HttpContext))
            {
                return RedirectToPage("/Index");
            }

            if (!AuthHelper.IsCliente(HttpContext))
            {
                return RedirectToPage("/Profissional/Dashboard");
            }

            if (string.IsNullOrEmpty(codigo))
            {
                return RedirectToPage("/Cliente/Agendar");
            }

            var usuarioId = AuthHelper.GetUsuarioId(HttpContext)!.Value;
            var agendamento = _context.Agendamentos
                .Include(a => a.Servico)
                .FirstOrDefault(a => a.CodigoConfirmacao == codigo && a.UsuarioId == usuarioId);

            if (agendamento == null)
            {
                return RedirectToPage("/Cliente/Agendar");
            }

            CodigoConfirmacao = agendamento.CodigoConfirmacao!;
            NomeServico = agendamento.Servico.Nome;
            DataHora = agendamento.DataHora;

            return Page();
        }
    }
}
