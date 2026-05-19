namespace TierLab.Domain.Entities;

public class Curtida
{
    public long TierlistId { get; set; }
    public Guid UsuarioId { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Tierlist Tierlist { get; set; } = null!;
    public Usuario Usuario { get; set; } = null!;
}
