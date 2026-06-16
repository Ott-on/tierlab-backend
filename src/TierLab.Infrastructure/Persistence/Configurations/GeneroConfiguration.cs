using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TierLab.Domain.Entities;

namespace TierLab.Infrastructure.Persistence.Configurations;

public class GeneroConfiguration : IEntityTypeConfiguration<Genero>
{
    public void Configure(EntityTypeBuilder<Genero> builder)
    {
        builder.ToTable("generos");
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Id).HasColumnName("id").UseIdentityAlwaysColumn();
        builder.Property(g => g.Nome).HasColumnName("nome").IsRequired();
    }
}
