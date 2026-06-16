using Microsoft.EntityFrameworkCore;
using TierLab.Domain.Common;
using TierLab.Domain.Entities;

namespace TierLab.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // ── Core Entities ─────────────────────────────────────
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Jogo> Jogos => Set<Jogo>();
    public DbSet<Genero> Generos => Set<Genero>();
    public DbSet<Plataforma> Plataformas => Set<Plataforma>();
    public DbSet<Tierlist> Tierlists => Set<Tierlist>();
    public DbSet<Comentario> Comentarios => Set<Comentario>();

    // ── Junction / Relationship Tables ────────────────────
    public DbSet<Curtida> Curtidas => Set<Curtida>();
    public DbSet<GeneroJogo> GeneroJogos => Set<GeneroJogo>();
    public DbSet<PlataformaJogo> PlataformaJogos => Set<PlataformaJogo>();
    public DbSet<TierJogo> TierJogos => Set<TierJogo>();
    public DbSet<UsuarioJogo> UsuarioJogos => Set<UsuarioJogo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply all IEntityTypeConfiguration<T> from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditInfo();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditInfo()
    {
        // Auto-audit for IAuditable entities
        var auditableEntries = ChangeTracker.Entries<IAuditable>();

        foreach (var entry in auditableEntries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CriadoEm = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.AtualizadoEm = DateTime.UtcNow;
            }
        }

        // Handle soft deletes
        var softDeleteEntries = ChangeTracker.Entries<ISoftDeletable>();

        foreach (var entry in softDeleteEntries)
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedAt = DateTime.UtcNow;
            }
        }
    }
}
