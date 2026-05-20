using TierLab.Application.UseCases.Tierlists.Dtos;

namespace TierLab.Application.UseCases.Tierlists;

public interface ITierlistReadRepository
{
    Task<IReadOnlyList<TierlistSummary>> GetByGeneroAsync(
        long generoId, int limit, CancellationToken ct = default);

    Task<IReadOnlyList<TierlistSummary>> SearchByTituloAsync(
        string query, int maxResults, CancellationToken ct = default);
}
