namespace TierLab.Domain.Entities;

public class PlataformaJogo
{
    public long JogoId { get; set; }
    public long PlataformaId { get; set; }

    // Navigation properties
    public Jogo Jogo { get; set; } = null!;
    public Plataforma Plataforma { get; set; } = null!;
}
