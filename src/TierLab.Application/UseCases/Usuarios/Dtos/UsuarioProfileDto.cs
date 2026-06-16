namespace TierLab.Application.UseCases.Usuarios.Dtos;

public sealed record UsuarioProfileDto(
    Guid Id,
    string Username,
    string? Email,
    string? ImageUrl,
    string? Bio,
    bool PerfilPublico,
    string Role,
    DateTime CriadoEm);
