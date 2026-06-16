using TierLab.Domain.Common;

namespace TierLab.Domain.Entities;

public class Usuario : Entity<Guid>
{
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Role { get; set; } = "user";
    public string? ImageUrl { get; set; }
    public string? Bio { get; set; }
    public bool PerfilPublico { get; set; } = true;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<Tierlist> Tierlists { get; set; } = [];
    public ICollection<Comentario> Comentarios { get; set; } = [];
    public ICollection<Curtida> Curtidas { get; set; } = [];
    public ICollection<UsuarioJogo> UsuarioJogos { get; set; } = [];
}
