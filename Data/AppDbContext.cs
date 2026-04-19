using Microsoft.EntityFrameworkCore;
using PIM2026.Models;

namespace PIM2026.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações para PostgreSQL
            modelBuilder.HasAnnotation("Npgsql:Collation", "pt_BR.utf8");

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuarios");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).UseIdentityByDefaultColumn();
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Email).HasMaxLength(200).IsRequired();
                entity.Property(u => u.Nome).HasMaxLength(200).IsRequired();
                entity.Property(u => u.Senha).HasMaxLength(500).IsRequired();
                entity.Property(u => u.Telefone).HasMaxLength(20).IsRequired();
                entity.Property(u => u.Perfil).HasMaxLength(50).HasDefaultValue("Cliente");
                entity.Property(u => u.IsActive).HasColumnName("isactive").HasDefaultValue(true);
                entity.Property(u => u.CreatedAt).HasColumnName("createdat").HasColumnType("timestamp with time zone").HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
                entity.Property(u => u.UpdatedAt).HasColumnName("updatedat").HasColumnType("timestamp with time zone").HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            });

            modelBuilder.Entity<Servico>(entity =>
            {
                entity.ToTable("servicos");
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Id).UseIdentityByDefaultColumn();
                entity.Property(s => s.Nome).HasMaxLength(200).IsRequired();
                entity.Property(s => s.Descricao).HasMaxLength(500);
                entity.Property(s => s.DuracaoMinutos).HasColumnName("duracaominutos").IsRequired();
                entity.Property(s => s.Preco).IsRequired();
                entity.Property(s => s.CorHex).HasColumnName("corhex").HasMaxLength(7).HasDefaultValue("#FF6B6B");
                entity.Property(s => s.IsActive).HasColumnName("isactive").HasDefaultValue(true);
                entity.Property(s => s.CreatedAt).HasColumnName("createdat").HasColumnType("timestamp with time zone").HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
                entity.Property(s => s.UpdatedAt).HasColumnName("updatedat").HasColumnType("timestamp with time zone").HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            });

            modelBuilder.Entity<Agendamento>(entity =>
            {
                entity.ToTable("agendamentos");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Id).UseIdentityByDefaultColumn();

                // --- AS DUAS LINHAS SALVADORAS AQUI ---
                entity.Property(a => a.UsuarioId).HasColumnName("usuarioid");
                entity.Property(a => a.ServicoId).HasColumnName("servicoid");
                // --------------------------------------

                entity.Property(a => a.Status).HasMaxLength(50).HasDefaultValue("Pendente");
                entity.Property(a => a.Observacoes).HasColumnName("observacoes").HasMaxLength(1000);
                entity.Property(a => a.CodigoConfirmacao).HasColumnName("codigoconfirmacao").HasMaxLength(50);
                entity.Property(a => a.MotivoCancelamento).HasColumnName("motivocancelamento").HasMaxLength(500);
                entity.Property(a => a.DataHora).HasColumnName("datahora").HasColumnType("timestamp with time zone");
                entity.Property(a => a.DataHoraFim).HasColumnName("datahorafim").HasColumnType("timestamp with time zone");
                entity.Property(a => a.DataCancelamento).HasColumnName("datacancelamento").HasColumnType("timestamp with time zone");
                entity.Property(a => a.CreatedAt).HasColumnName("createdat").HasColumnType("timestamp with time zone").HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
                entity.Property(a => a.UpdatedAt).HasColumnName("updatedat").HasColumnType("timestamp with time zone").HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

                entity.HasIndex(a => a.DataHora);
                entity.HasOne(a => a.Cliente).WithMany(u => u.Agendamentos).HasForeignKey(a => a.UsuarioId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(a => a.Servico).WithMany(s => s.Agendamentos).HasForeignKey(a => a.ServicoId).OnDelete(DeleteBehavior.Cascade);
            });
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Usuario || e.Entity is Servico || e.Entity is Agendamento);

            foreach (var entry in entries)
            {
                var property = entry.Entity.GetType().GetProperty("UpdatedAt");
                if (property != null && entry.State == EntityState.Modified)
                {
                    property.SetValue(entry.Entity, DateTime.UtcNow);
                }
            }
        }
    }
}