using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TierLab.Domain.Entities;

namespace TierLab.Infrastructure.Persistence.Configurations;

public class JogoConfiguration : IEntityTypeConfiguration<Jogo>
{
    public void Configure(EntityTypeBuilder<Jogo> builder)
    {
        builder.ToTable("jogos");
        builder.HasKey(j => j.Id);
        builder.Property(j => j.Id).HasColumnName("id").UseIdentityAlwaysColumn();
        builder.Property(j => j.Titulo).HasColumnName("titulo").IsRequired();
        builder.Property(j => j.ImageUrl).HasColumnName("image_url");
        builder.Property(j => j.AnoLancamento).HasColumnName("ano_lancamento");
    }
}
