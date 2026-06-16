using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TierLab.Domain.Entities;

namespace TierLab.Infrastructure.Persistence.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usuarios");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnName("id");
        builder.Property(u => u.Username).HasColumnName("username").IsRequired();
        builder.Property(u => u.Email).HasColumnName("email");
        builder.Property(u => u.Role).HasColumnName("role").IsRequired().HasDefaultValue("user");
        builder.Property(u => u.ImageUrl).HasColumnName("image_url");
        builder.Property(u => u.Bio).HasColumnName("bio");
        builder.Property(u => u.PerfilPublico).HasColumnName("perfil_publico").HasDefaultValue(true);
        builder.Property(u => u.CriadoEm).HasColumnName("criado_em").HasDefaultValueSql("now()");
    }
}
