using TierLab.Application.Common;
using TierLab.Application.UseCases.Usuarios.Dtos;
using TierLab.Application.UseCases.Usuarios.Requests;

namespace TierLab.Application.UseCases.Usuarios;

public interface IUsuarioService
{
    Task<Result<UsuarioProfileDto>> GetOrCreateCurrentAsync(
        Guid id,
        string? email,
        string? username,
        string? imageUrl,
        CancellationToken ct = default);

    Task<Result<UsuarioProfileDto>> GetByIdAsync(Guid id, Guid? requesterId, CancellationToken ct = default);

    Task<Result<UsuarioProfileDto>> UpdateProfileAsync(Guid id, UpdateUsuarioRequest request, CancellationToken ct = default);

    Task<Result> AddJogoAsync(Guid usuarioId, AddUsuarioJogoRequest request, CancellationToken ct = default);

    Task<Result<IReadOnlyList<UsuarioJogoResponse>>> ListJogosAsync(Guid usuarioId, CancellationToken ct = default);
    Task<Result> RemoveJogoAsync(Guid usuarioId, long jogoId, CancellationToken ct = default);
    Task<Result> UpdateJogoAsync(Guid usuarioId, long jogoId, UpdateUsuarioJogoRequest request, CancellationToken ct = default);
}
