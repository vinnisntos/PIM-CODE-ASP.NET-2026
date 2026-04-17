using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIM2026.Data;
using PIM2026.Models;

namespace PIM2026.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            // Carrega a página inicial
        }

        // Método acionado pelo botão de Entrar
        public IActionResult OnPost(string Email, string Senha)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == Email && u.Senha == Senha);

            if (usuario != null)
            {
                // Login deu certo. Mais pra frente vamos criar a sessão/cookie aqui.
                return RedirectToPage("/Agendamento");
            }

            ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
            return Page();
        }

        // Método acionado pelo botão de Cadastrar
        public IActionResult OnPostRegister(string Nome, string Email, string Senha)
        {
            if (_context.Usuarios.Any(u => u.Email == Email))
            {
                ModelState.AddModelError(string.Empty, "E-mail já cadastrado.");
                return Page();
            }

            var novoUsuario = new Usuario
            {
                Nome = Nome,
                Email = Email,
                Senha = Senha, // Para o PIM, vamos deixar em texto puro por enquanto
                Perfil = "Cliente"
            };

            _context.Usuarios.Add(novoUsuario);
            _context.SaveChanges();

            // Cadastrou com sucesso, vai direto pro agendamento
            return RedirectToPage("/Agendamento");
        }
    }
}