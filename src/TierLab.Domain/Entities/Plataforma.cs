using TierLab.Domain.Common;

namespace TierLab.Domain.Entities;

public class Plataforma : Entity<long>
{
    public string Nome { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<PlataformaJogo> PlataformaJogos { get; set; } = [];
}
