using TierLab.Application.Common;
using TierLab.Application.UseCases.Tierlists.Dtos;
using TierLab.Application.UseCases.Jogos.Dtos;

namespace TierLab.Application.UseCases.Tierlists;

public interface ITierlistReadRepository
{
    Task<CursorPage<TierlistCursorItem>> GetTierlistsCursorPageAsync(
        int limit, CursorPageKey? cursor, CancellationToken ct = default);

    Task<IReadOnlyList<TierlistSummary>> GetByGeneroAsync(
        long generoId, int limit, CancellationToken ct = default);

    Task<IReadOnlyList<TierlistSummary>> GetByUsuarioAsync(Guid usuarioId, int limit, CancellationToken ct = default);

    Task<IReadOnlyList<TierlistSummary>> SearchByTituloAsync(
        string query, int maxResults, CancellationToken ct = default);

    Task<TierlistSummary?> GetByIdAsync(long id, CancellationToken ct = default);

    Task<IReadOnlyList<JogoTierResponse>> GetJogosByTierlistIdAsync(long tierlistId, CancellationToken ct = default);

    Task<CursorPage<JogoTierResponse>> GetJogosCursorPageAsync(
        int limit, CursorPageKey? cursor, long tierlistId, CancellationToken ct = default);
}
