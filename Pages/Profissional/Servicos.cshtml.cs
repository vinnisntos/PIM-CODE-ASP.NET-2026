using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIM2026.Data;
using PIM2026.Models;
using PIM2026.Services;

namespace PIM2026.Pages.Profissional
{
    public class ServicosModel : PageModel
    {
        private readonly AppDbContext _context;

        public ServicosModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Servico> Servicos { get; set; } = new();

        [BindProperty]
        public Servico NovoServico { get; set; } = new();

        [BindProperty]
        public Servico? ServicoEdicao { get; set; }

        public string? Mensagem { get; set; }

        public IActionResult OnGet()
        {
            if (!AuthHelper.IsAuthenticated(HttpContext) || !AuthHelper.IsProfissional(HttpContext))
            {
                return RedirectToPage("/Index");
            }

            Servicos = _context.Servicos.OrderBy(s => s.Nome).ToList();
            return Page();
        }

        public IActionResult OnPostAdicionar()
        {
            if (!AuthHelper.IsProfissional(HttpContext))
            {
                return RedirectToPage("/Index");
            }

            if (!ModelState.IsValid)
            {
                Servicos = _context.Servicos.OrderBy(s => s.Nome).ToList();
                return Page();
            }

            NovoServico.IsActive = true;
            NovoServico.CreatedAt = DateTime.UtcNow;
            NovoServico.UpdatedAt = DateTime.UtcNow;

            _context.Servicos.Add(NovoServico);
            _context.SaveChanges();

            return RedirectToPage();
        }

        public IActionResult OnPostEditar(int id)
        {
            if (!AuthHelper.IsProfissional(HttpContext))
            {
                return RedirectToPage("/Index");
            }

            var servico = _context.Servicos.Find(id);
            if (servico == null)
            {
                return RedirectToPage();
            }

            if (ServicoEdicao != null)
            {
                servico.Nome = ServicoEdicao.Nome;
                servico.Descricao = ServicoEdicao.Descricao;
                servico.DuracaoMinutos = ServicoEdicao.DuracaoMinutos;
                servico.Preco = ServicoEdicao.Preco;
                servico.CorHex = ServicoEdicao.CorHex;
                servico.UpdatedAt = DateTime.UtcNow;

                _context.SaveChanges();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostAtivarDesativar(int id)
        {
            if (!AuthHelper.IsProfissional(HttpContext))
            {
                return RedirectToPage("/Index");
            }

            var servico = _context.Servicos.Find(id);
            if (servico != null)
            {
                servico.IsActive = !servico.IsActive;
                servico.UpdatedAt = DateTime.UtcNow;
                _context.SaveChanges();
            }

            return RedirectToPage();
        }
    }
}
