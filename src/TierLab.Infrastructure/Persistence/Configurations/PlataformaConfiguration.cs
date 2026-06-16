using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TierLab.Domain.Entities;

namespace TierLab.Infrastructure.Persistence.Configurations;

public class PlataformaConfiguration : IEntityTypeConfiguration<Plataforma>
{
    public void Configure(EntityTypeBuilder<Plataforma> builder)
    {
        builder.ToTable("plataformas");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id").UseIdentityAlwaysColumn();
        builder.Property(p => p.Nome).HasColumnName("nome").IsRequired();
    }
}
