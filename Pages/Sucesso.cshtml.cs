using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PIM2026.Pages
{
    public class SucessoModel : PageModel
    {
        public string CodigoConfirmacao { get; set; } = "SB-77G2XP"; // Simulação de código

        public void OnGet()
        {
            // Aqui poderíamos buscar o último agendamento do banco para exibir os detalhes reais
        }
    }
}