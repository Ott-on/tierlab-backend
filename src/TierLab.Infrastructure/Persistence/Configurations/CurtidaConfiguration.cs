using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TierLab.Domain.Entities;

namespace TierLab.Infrastructure.Persistence.Configurations;

public class CurtidaConfiguration : IEntityTypeConfiguration<Curtida>
{
    public void Configure(EntityTypeBuilder<Curtida> builder)
    {
        builder.ToTable("curtidas");
        builder.HasKey(c => new { c.TierlistId, c.UsuarioId });
        builder.Property(c => c.TierlistId).HasColumnName("tierlist_id");
        builder.Property(c => c.UsuarioId).HasColumnName("usuario_id");
        builder.Property(c => c.CriadoEm).HasColumnName("criado_em").HasDefaultValueSql("now()");

        builder.HasOne(c => c.Tierlist)
            .WithMany(t => t.Curtidas)
            .HasForeignKey(c => c.TierlistId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Usuario)
            .WithMany(u => u.Curtidas)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
