namespace TierLab.Application.UseCases.Usuarios.Dtos;

public sealed record UsuarioJogoResponse(
    long JogoId,
    string? Status,
    int? Nota,
    int HorasJogadas,
    DateTime AdicionadoEm);