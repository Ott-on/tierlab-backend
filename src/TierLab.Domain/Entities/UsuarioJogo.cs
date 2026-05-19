namespace TierLab.Domain.Entities;

public class UsuarioJogo
{
    public Guid UsuarioId { get; set; }
    public long JogoId { get; set; }
    public string? Status { get; set; }
    public int? Nota { get; set; }
    public int HorasJogadas { get; set; }
    public DateTime AdicionadoEm { get; set; } = DateTime.UtcNow;
    public DateTime? AtualizadoEm { get; set; }

    // Navigation properties
    public Usuario Usuario { get; set; } = null!;
    public Jogo Jogo { get; set; } = null!;
}
