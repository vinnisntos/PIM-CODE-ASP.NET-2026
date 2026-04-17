using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIM2026.Data;
using PIM2026.Models;

namespace PIM2026.Pages
{
    public class AgendamentoModel : PageModel
    {
        private readonly AppDbContext _context;

        public AgendamentoModel(AppDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            // Quando a página carrega, não fazemos nada por enquanto.
            // Futuramente, puxaremos os serviços do banco aqui.
        }

        public IActionResult OnPost(string Profissional, string Data, string Hora, string ServicoResumo)
        {
            // 1. Simulação: Pegar o usuário logado (depois faremos com Sessão real)
            var cliente = _context.Usuarios.FirstOrDefault();

            if (cliente == null)
            {
                return RedirectToPage("/Index"); // Manda pro login se der erro
            }

            // 2. Salva o agendamento no banco
            var novoAgendamento = new Agendamento
            {
                UsuarioId = cliente.Id,
                DataHora = DateTime.Now, // Aqui você precisará converter a string de Data/Hora do JS pra DateTime no futuro
                Status = "Confirmado",
                // ServicoId = 1 // Precisaremos mapear o serviço real depois
            };

            _context.Agendamentos.Add(novoAgendamento);
            _context.SaveChanges();

            // 3. Redireciona para a tela de Sucesso
            return RedirectToPage("/Sucesso");
        }
    }
}