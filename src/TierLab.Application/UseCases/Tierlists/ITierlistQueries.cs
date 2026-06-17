using TierLab.Application.Common;
using TierLab.Application.Common;
using TierLab.Application.UseCases.Tierlists.Dtos;
using TierLab.Application.UseCases.Tierlists.Requests;

namespace TierLab.Application.UseCases.Tierlists;

public interface ITierlistQueries
{
    Task<Result<CursorPageResponse<TierlistCursorItem>>> ListByCursorAsync(
        TierlistsCursorRequest request, CancellationToken ct = default);

    Task<Result<IReadOnlyList<TierlistSummary>>> ListByGeneroAsync(
        long generoId, int limit, CancellationToken ct = default);

    Task<Result<IReadOnlyList<TierlistSummary>>> ListByUsuarioAsync(Guid usuarioId, int limit, CancellationToken ct = default);

    Task<Result<IReadOnlyList<TierlistSummary>>> SearchAsync(
        string query, CancellationToken ct = default);

    Task<Result<TierlistSummary>> GetByIdAsync(long id, CancellationToken ct = default);

    Task<Result<CursorPageResponse<TierLab.Application.UseCases.Jogos.Dtos.JogoTierResponse>>> ListJogosByCursorAsync(
        long tierlistId, int limit, string? cursor, CancellationToken ct = default);

    Task<Result<IReadOnlyList<TierLab.Application.UseCases.Jogos.Dtos.JogoTierResponse>>> ListJogosByTierlistIdAsync(long tierlistId, CancellationToken ct = default);
}
