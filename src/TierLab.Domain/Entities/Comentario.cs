using TierLab.Domain.Common;

namespace TierLab.Domain.Entities;

public class Comentario : Entity<long>
{
    public long TierlistId { get; set; }
    public Guid UsuarioId { get; set; }
    public string Conteudo { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Tierlist Tierlist { get; set; } = null!;
    public Usuario Usuario { get; set; } = null!;
}
