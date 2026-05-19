using TierLab.Domain.Common;

namespace TierLab.Domain.Entities;

public class Tierlist : Entity<long>, IAuditable
{
    public Guid UsuarioId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? Descricao { get; set; }
    public string Visibilidade { get; set; } = "public";
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    public DateTime? AtualizadoEm { get; set; }

    // Navigation properties
    public Usuario Usuario { get; set; } = null!;
    public ICollection<TierJogo> TierJogos { get; set; } = [];
    public ICollection<Comentario> Comentarios { get; set; } = [];
    public ICollection<Curtida> Curtidas { get; set; } = [];
}
