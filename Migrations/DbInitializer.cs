using PIM2026.Data;
using PIM2026.Models;
using PIM2026.Services;

namespace PIM2026.Migrations
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Servicos.Any())
            {
                return;
            }

            var servicos = new Servico[]
            {
                new Servico { Nome = "Design de Sobrancelha", Descricao = "Modelagem e design personalizado da sobrancelha", DuracaoMinutos = 30, Preco = 45.00m, CorHex = "#8B4513" },
                new Servico { Nome = "Henna na Sobrancelha", Descricao = "Coloração natural com henna", DuracaoMinutos = 45, Preco = 65.00m, CorHex = "#D2691E" },
                new Servico { Nome = "Design + Henna", Descricao = "Combo de design e henna", DuracaoMinutos = 60, Preco = 95.00m, CorHex = "#A0522D" },
                new Servico { Nome = "Microblading", Descricao = "Técnica de micropigmentação fio a fio", DuracaoMinutos = 120, Preco = 450.00m, CorHex = "#654321" },
                new Servico { Nome = "Retoque Microblading", Descricao = "Retoque de microblading após 30 dias", DuracaoMinutos = 90, Preco = 150.00m, CorHex = "#8B7355" },
                new Servico { Nome = "Depilação Facial", Descricao = "Depilação com cera em áreas do rosto", DuracaoMinutos = 30, Preco = 35.00m, CorHex = "#FFB6C1" }
            };

            context.Servicos.AddRange(servicos);

            var profissional = new Usuario
            {
                Nome = "Administrador",
                Email = "admin@studiobeauty.com",
                Senha = PasswordHasher.HashPassword("Admin@123"),
                Telefone = "(11) 99999-9999",
                Perfil = "Profissional",
                IsActive = true
            };

            context.Usuarios.Add(profissional);

            context.SaveChanges();
        }
    }
}
