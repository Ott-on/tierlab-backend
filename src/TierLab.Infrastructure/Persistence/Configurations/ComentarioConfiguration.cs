using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TierLab.Domain.Entities;

namespace TierLab.Infrastructure.Persistence.Configurations;

public class ComentarioConfiguration : IEntityTypeConfiguration<Comentario>
{
    public void Configure(EntityTypeBuilder<Comentario> builder)
    {
        builder.ToTable("comentarios");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id").UseIdentityAlwaysColumn();
        builder.Property(c => c.TierlistId).HasColumnName("tierlist_id").IsRequired();
        builder.Property(c => c.UsuarioId).HasColumnName("usuario_id").IsRequired();
        builder.Property(c => c.Conteudo).HasColumnName("conteudo").IsRequired();
        builder.Property(c => c.CriadoEm).HasColumnName("criado_em").HasDefaultValueSql("now()");

        builder.HasOne(c => c.Tierlist)
            .WithMany(t => t.Comentarios)
            .HasForeignKey(c => c.TierlistId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Usuario)
            .WithMany(u => u.Comentarios)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
