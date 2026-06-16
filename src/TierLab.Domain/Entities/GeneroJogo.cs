namespace TierLab.Domain.Entities;

public class GeneroJogo
{
    public long JogoId { get; set; }
    public long GeneroId { get; set; }

    // Navigation properties
    public Jogo Jogo { get; set; } = null!;
    public Genero Genero { get; set; } = null!;
}
