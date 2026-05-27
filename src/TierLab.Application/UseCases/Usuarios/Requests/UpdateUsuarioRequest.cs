namespace TierLab.Application.UseCases.Usuarios.Requests;

public sealed class UpdateUsuarioRequest
{
    public string? Username { get; set; }
    public string? ImageUrl { get; set; }
    public string? Bio { get; set; }
    public bool? PerfilPublico { get; set; }
}
