using TierLab.Domain.Common;

namespace TierLab.Domain.Entities;

public class Genero : Entity<long>
{
    public string Nome { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<GeneroJogo> GeneroJogos { get; set; } = [];
}
