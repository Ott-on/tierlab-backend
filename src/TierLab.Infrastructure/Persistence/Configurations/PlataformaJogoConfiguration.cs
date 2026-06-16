using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TierLab.Domain.Entities;

namespace TierLab.Infrastructure.Persistence.Configurations;

public class PlataformaJogoConfiguration : IEntityTypeConfiguration<PlataformaJogo>
{
    public void Configure(EntityTypeBuilder<PlataformaJogo> builder)
    {
        builder.ToTable("plataforma_jogo");
        builder.HasKey(pj => new { pj.JogoId, pj.PlataformaId });
        builder.Property(pj => pj.JogoId).HasColumnName("jogo_id");
        builder.Property(pj => pj.PlataformaId).HasColumnName("plataforma_id");

        builder.HasOne(pj => pj.Jogo)
            .WithMany(j => j.PlataformaJogos)
            .HasForeignKey(pj => pj.JogoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pj => pj.Plataforma)
            .WithMany(p => p.PlataformaJogos)
            .HasForeignKey(pj => pj.PlataformaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
