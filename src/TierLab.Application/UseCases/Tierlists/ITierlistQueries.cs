using TierLab.Application.Common;
using TierLab.Application.UseCases.Tierlists.Dtos;

namespace TierLab.Application.UseCases.Tierlists;

public interface ITierlistQueries
{
    Task<Result<IReadOnlyList<TierlistSummary>>> ListByGeneroAsync(
        long generoId, int limit, CancellationToken ct = default);

    Task<Result<IReadOnlyList<TierlistSummary>>> SearchAsync(
        string query, CancellationToken ct = default);
}
