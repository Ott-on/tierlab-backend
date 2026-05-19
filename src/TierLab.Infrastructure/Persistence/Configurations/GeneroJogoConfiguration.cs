using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TierLab.Domain.Entities;

namespace TierLab.Infrastructure.Persistence.Configurations;

public class GeneroJogoConfiguration : IEntityTypeConfiguration<GeneroJogo>
{
    public void Configure(EntityTypeBuilder<GeneroJogo> builder)
    {
        builder.ToTable("genero_jogo");
        builder.HasKey(gj => new { gj.JogoId, gj.GeneroId });
        builder.Property(gj => gj.JogoId).HasColumnName("jogo_id");
        builder.Property(gj => gj.GeneroId).HasColumnName("genero_id");

        builder.HasOne(gj => gj.Jogo)
            .WithMany(j => j.GeneroJogos)
            .HasForeignKey(gj => gj.JogoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(gj => gj.Genero)
            .WithMany(g => g.GeneroJogos)
            .HasForeignKey(gj => gj.GeneroId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
