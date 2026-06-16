namespace TierLab.Application.UseCases.Tierlists.Requests;

public sealed class CreateTierlistRequest
{
    public Guid UsuarioId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? ImageUrl { get; set; }
    public string? Visibilidade { get; set; }
}
