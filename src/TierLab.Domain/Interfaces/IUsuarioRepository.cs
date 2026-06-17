using TierLab.Domain.Entities;

namespace TierLab.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Usuario> AddAsync(Usuario usuario, CancellationToken cancellationToken = default);
    Task UpdateAsync(Usuario usuario, CancellationToken cancellationToken = default);

    Task<bool> JogoExistsAsync(long jogoId, CancellationToken cancellationToken = default);
    Task<bool> UsuarioJogoExistsAsync(Guid usuarioId, long jogoId, CancellationToken cancellationToken = default);
    Task AddUsuarioJogoAsync(UsuarioJogo usuarioJogo, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<UsuarioJogo>> GetUsuarioJogosAsync(Guid usuarioId, CancellationToken cancellationToken = default);
    Task<UsuarioJogo?> GetUsuarioJogoAsync(Guid usuarioId, long jogoId, CancellationToken cancellationToken = default);
    Task RemoveUsuarioJogoAsync(UsuarioJogo usuarioJogo, CancellationToken cancellationToken = default);
    Task UpdateUsuarioJogoAsync(UsuarioJogo usuarioJogo, CancellationToken cancellationToken = default);
}
