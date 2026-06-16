namespace TierLab.Domain.Entities;

public class TierJogo
{
    public long TierlistId { get; set; }
    public long JogoId { get; set; }
    public string Tier { get; set; } = string.Empty;
    public int Posicao { get; set; }
    public DateTime AdicionadoEm { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Tierlist Tierlist { get; set; } = null!;
    public Jogo Jogo { get; set; } = null!;
}
