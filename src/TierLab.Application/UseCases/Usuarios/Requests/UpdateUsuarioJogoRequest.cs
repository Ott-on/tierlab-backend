namespace TierLab.Application.UseCases.Usuarios.Requests;

public sealed class UpdateUsuarioJogoRequest
{
    public string? Status { get; set; }
    public int? Nota { get; set; }
    public int? HorasJogadas { get; set; }
}
