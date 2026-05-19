using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TierLab.Domain.Entities;

namespace TierLab.Infrastructure.Persistence.Configurations;

public class TierlistConfiguration : IEntityTypeConfiguration<Tierlist>
{
    public void Configure(EntityTypeBuilder<Tierlist> builder)
    {
        builder.ToTable("tierlists");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasColumnName("id").UseIdentityAlwaysColumn();
        builder.Property(t => t.UsuarioId).HasColumnName("usuario_id").IsRequired();
        builder.Property(t => t.Titulo).HasColumnName("titulo").IsRequired();
        builder.Property(t => t.ImageUrl).HasColumnName("image_url");
        builder.Property(t => t.Descricao).HasColumnName("descricao");
        builder.Property(t => t.Visibilidade).HasColumnName("visibilidade").IsRequired().HasDefaultValue("public");
        builder.Property(t => t.CriadoEm).HasColumnName("criado_em").HasDefaultValueSql("now()");
        builder.Property(t => t.AtualizadoEm).HasColumnName("atualizado_em").HasDefaultValueSql("now()");

        builder.HasOne(t => t.Usuario)
            .WithMany(u => u.Tierlists)
            .HasForeignKey(t => t.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
