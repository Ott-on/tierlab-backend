using TierLab.Domain.Common;

namespace TierLab.Domain.Entities;

public class Jogo : Entity<long>
{
    public int? ApiId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int? AnoLancamento { get; set; }

    // Navigation properties
    public ICollection<GeneroJogo> GeneroJogos { get; set; } = [];
    public ICollection<PlataformaJogo> PlataformaJogos { get; set; } = [];
    public ICollection<TierJogo> TierJogos { get; set; } = [];
    public ICollection<UsuarioJogo> UsuarioJogos { get; set; } = [];
}
