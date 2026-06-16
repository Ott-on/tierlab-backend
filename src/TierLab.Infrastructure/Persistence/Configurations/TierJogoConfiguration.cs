using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TierLab.Domain.Entities;

namespace TierLab.Infrastructure.Persistence.Configurations;

public class TierJogoConfiguration : IEntityTypeConfiguration<TierJogo>
{
    public void Configure(EntityTypeBuilder<TierJogo> builder)
    {
        builder.ToTable("tier_jogo");
        builder.HasKey(tj => new { tj.TierlistId, tj.JogoId });
        builder.Property(tj => tj.TierlistId).HasColumnName("tierlist_id");
        builder.Property(tj => tj.JogoId).HasColumnName("jogo_id");
        builder.Property(tj => tj.Tier).HasColumnName("tier").IsRequired();
        builder.Property(tj => tj.Posicao).HasColumnName("posicao").HasDefaultValue(0);
        builder.Property(tj => tj.AdicionadoEm).HasColumnName("adicionado_em").HasDefaultValueSql("now()");

        builder.HasOne(tj => tj.Tierlist)
            .WithMany(t => t.TierJogos)
            .HasForeignKey(tj => tj.TierlistId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(tj => tj.Jogo)
            .WithMany(j => j.TierJogos)
            .HasForeignKey(tj => tj.JogoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
