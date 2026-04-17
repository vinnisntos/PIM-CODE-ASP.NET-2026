using Microsoft.EntityFrameworkCore;
using PIM2026.Data;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os serviços do Razor Pages
builder.Services.AddRazorPages();

// Configura o banco de dados (SQL Server)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();