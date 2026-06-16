using TierLab.Application.Common;
using TierLab.Application.UseCases.Tierlists.Dtos;

namespace TierLab.Application.UseCases.Tierlists;

public sealed class TierlistQueries : ITierlistQueries
{
    private readonly ITierlistReadRepository _repository;

    public TierlistQueries(ITierlistReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IReadOnlyList<TierlistSummary>>> ListByGeneroAsync(
        long generoId, int limit, CancellationToken ct = default)
    {
        // limit <= 0 means "all"
        limit = limit > 0 ? Math.Min(limit, 100) : 0;

        var items = await _repository.GetByGeneroAsync(generoId, limit, ct);
        return Result<IReadOnlyList<TierlistSummary>>.Ok(items);
    }

    public async Task<Result<IReadOnlyList<TierlistSummary>>> SearchAsync(
        string query, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
            return Result<IReadOnlyList<TierlistSummary>>.Ok([]);

        var items = await _repository.SearchByTituloAsync(query.Trim(), maxResults: 10, ct);
        return Result<IReadOnlyList<TierlistSummary>>.Ok(items);
    }

    public async Task<Result<TierlistSummary>> GetByIdAsync(long id, CancellationToken ct = default)
    {
        var item = await _repository.GetByIdAsync(id, ct);
        if (item is null)
            return Result<TierlistSummary>.Fail("Tierlist não encontrada.");

        return Result<TierlistSummary>.Ok(item);
    }
}
