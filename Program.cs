using Microsoft.EntityFrameworkCore;
using EFCore.NamingConventions;
using PIM2026.Data;
using PIM2026.Services;
using PIM2026.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Adiciona Razor Pages
builder.Services.AddRazorPages();

// Configuração do PostgreSQL/Supabase
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions =>
        {
            npgsqlOptions.MigrationsHistoryTable("__ef_migrations_history", "public");
        }
    ).UseCamelCaseNamingConvention());

// Configuração de Sessão (AJUSTADA)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    // Mudamos para 'SameAsRequest' para evitar o Erro 400 no localhost
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.Name = ".StudioBeauty.Session";
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AgendamentoService>();

var app = builder.Build();

// Middlewares
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// A ordem está correta: Session antes de Authorization
app.UseSession();
app.UseAuthorization();

app.MapRazorPages();

// Inicialização do Banco
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    // As tabelas já foram criadas via script SQL no Supabase
    // Não executamos Migrate() para evitar conflitos, mas verificamos/criamos os dados iniciais
    DbInitializer.Initialize(context);
}

app.Run();