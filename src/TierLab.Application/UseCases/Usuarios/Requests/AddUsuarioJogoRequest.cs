namespace TierLab.Application.UseCases.Usuarios.Requests;

public sealed class AddUsuarioJogoRequest
{
    public long JogoId { get; set; }
    public string? Status { get; set; }
    public int? Nota { get; set; }
    public int? HorasJogadas { get; set; }
}
