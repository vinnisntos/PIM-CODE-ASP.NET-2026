using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIM2026.Data;
using PIM2026.Models;
using PIM2026.Services;

namespace PIM2026.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public string ErrorMessage { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        // Recebe os dados exatos do HTML (Email e Senha)
        public IActionResult OnPostLogin(string Email, string Senha)
        {
            // Busca o usuário no Supabase
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == Email && u.IsActive);

            // Verifica se achou e se a senha bate
            if (usuario == null || !PasswordHasher.VerifyPassword(Senha, usuario.Senha))
            {
                ErrorMessage = "E-mail ou senha inválidos. Tente novamente.";
                return Page(); // Isso recarrega a página mostrando a caixa vermelha!
            }

            // Salva na sessão
            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
            HttpContext.Session.SetString("UsuarioNome", usuario.Nome);
            HttpContext.Session.SetString("UsuarioPerfil", usuario.Perfil);

            // Redireciona
            if (usuario.Perfil == "Profissional")
            {
                return RedirectToPage("/Profissional/Dashboard");
            }

            return RedirectToPage("/Cliente/Agendar");
        }

        // Recebe os dados exatos do HTML de Cadastro
        public IActionResult OnPostRegister(string Nome, string Email, string Telefone, string Senha)
        {
            if (_context.Usuarios.Any(u => u.Email == Email))
            {
                ErrorMessage = "Este e-mail já está em uso.";
                return Page();
            }

            var novoUsuario = new Usuario
            {
                Nome = Nome,
                Email = Email,
                Senha = PasswordHasher.HashPassword(Senha),
                Telefone = Telefone,
                Perfil = "Cliente"
            };

            _context.Usuarios.Add(novoUsuario);
            _context.SaveChanges();

            HttpContext.Session.SetInt32("UsuarioId", novoUsuario.Id);
            HttpContext.Session.SetString("UsuarioNome", novoUsuario.Nome);
            HttpContext.Session.SetString("UsuarioPerfil", novoUsuario.Perfil);

            return RedirectToPage("/Cliente/Agendar");
        }
    }
}