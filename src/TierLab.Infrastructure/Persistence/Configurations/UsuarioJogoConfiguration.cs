using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TierLab.Domain.Entities;

namespace TierLab.Infrastructure.Persistence.Configurations;

public class UsuarioJogoConfiguration : IEntityTypeConfiguration<UsuarioJogo>
{
    public void Configure(EntityTypeBuilder<UsuarioJogo> builder)
    {
        builder.ToTable("usuario_jogo");
        builder.HasKey(uj => new { uj.UsuarioId, uj.JogoId });
        builder.Property(uj => uj.UsuarioId).HasColumnName("usuario_id");
        builder.Property(uj => uj.JogoId).HasColumnName("jogo_id");
        builder.Property(uj => uj.Status).HasColumnName("status");
        builder.Property(uj => uj.Nota).HasColumnName("nota");
        builder.Property(uj => uj.HorasJogadas).HasColumnName("horas_jogadas").HasDefaultValue(0);
        builder.Property(uj => uj.AdicionadoEm).HasColumnName("adicionado_em").HasDefaultValueSql("now()");
        builder.Property(uj => uj.AtualizadoEm).HasColumnName("atualizado_em").HasDefaultValueSql("now()");

        builder.HasOne(uj => uj.Usuario)
            .WithMany(u => u.UsuarioJogos)
            .HasForeignKey(uj => uj.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(uj => uj.Jogo)
            .WithMany(j => j.UsuarioJogos)
            .HasForeignKey(uj => uj.JogoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
