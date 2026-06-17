using TierLab.Application.Common;
using TierLab.Application.UseCases.Tierlists.Dtos;
using TierLab.Application.UseCases.Tierlists.Requests;
using TierLab.Application.UseCases.Jogos.Dtos;
using TierLab.Application.Common;

namespace TierLab.Application.UseCases.Tierlists;

public sealed class TierlistQueries : ITierlistQueries
{
    private readonly ITierlistReadRepository _repository;

    public TierlistQueries(ITierlistReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<CursorPageResponse<TierlistCursorItem>>> ListByCursorAsync(
        TierlistsCursorRequest request, CancellationToken ct = default)
    {
        const int maxPageSize = 100;
        const int defaultPageSize = 20;

        var limit = request.Limit <= 0 ? defaultPageSize : request.Limit;
        if (limit > maxPageSize)
            return Result<CursorPageResponse<TierlistCursorItem>>.Fail($"Limit máximo é {maxPageSize}.");

        CursorPageKey? cursor = null;
        if (!string.IsNullOrWhiteSpace(request.Cursor) &&
            !CursorSerializer.TryDecode(request.Cursor, out cursor))
        {
            return Result<CursorPageResponse<TierlistCursorItem>>.Fail("Cursor inválido.");
        }

        var page = await _repository.GetTierlistsCursorPageAsync(limit, cursor, ct);
        var nextCursor = page.HasMore && page.LastKey is not null
            ? CursorSerializer.Encode(page.LastKey)
            : null;

        return Result<CursorPageResponse<TierlistCursorItem>>.Ok(
            new CursorPageResponse<TierlistCursorItem>(page.Items, nextCursor, page.HasMore));
    }

    public async Task<Result<IReadOnlyList<TierlistSummary>>> ListByGeneroAsync(
        long generoId, int limit, CancellationToken ct = default)
    {
        // limit <= 0 means "all"
        limit = limit > 0 ? Math.Min(limit, 100) : 0;

        var items = await _repository.GetByGeneroAsync(generoId, limit, ct);
        return Result<IReadOnlyList<TierlistSummary>>.Ok(items);
    }

    public async Task<Result<IReadOnlyList<TierlistSummary>>> ListByUsuarioAsync(Guid usuarioId, int limit, CancellationToken ct = default)
    {
        // limit <= 0 means "all"
        limit = limit > 0 ? Math.Min(limit, 100) : 0;

        var items = await _repository.GetByUsuarioAsync(usuarioId, limit, ct);
        return Result<IReadOnlyList<TierlistSummary>>.Ok(items);
    }

    public async Task<Result<CursorPageResponse<JogoTierResponse>>> ListJogosByCursorAsync(long tierlistId, int limit, string? cursor, CancellationToken ct = default)
    {
        const int maxPageSize = 100;
        const int defaultPageSize = 20;

        var pageLimit = limit <= 0 ? defaultPageSize : limit;
        if (pageLimit > maxPageSize)
            return Result<CursorPageResponse<JogoTierResponse>>.Fail($"Limit máximo é {maxPageSize}.");

        CursorPageKey? key = null;
        if (!string.IsNullOrWhiteSpace(cursor) && !CursorSerializer.TryDecode(cursor, out key))
        {
            return Result<CursorPageResponse<JogoTierResponse>>.Fail("Cursor inválido.");
        }

        var page = await _repository.GetJogosCursorPageAsync(pageLimit, key, tierlistId, ct);
        var nextCursor = page.HasMore && page.LastKey is not null
            ? CursorSerializer.Encode(page.LastKey)
            : null;

        return Result<CursorPageResponse<JogoTierResponse>>.Ok(new CursorPageResponse<JogoTierResponse>(page.Items, nextCursor, page.HasMore));
    }

    public async Task<Result<IReadOnlyList<TierlistSummary>>> SearchAsync(
        string query, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
            return Result<IReadOnlyList<TierlistSummary>>.Ok([]);

        var items = await _repository.SearchByTituloAsync(query.Trim(), maxResults: 10, ct);
        return Result<IReadOnlyList<TierlistSummary>>.Ok(items);
    }

    public async Task<Result<TierlistDetail>> GetByIdAsync(long id, CancellationToken ct = default)
    {
        var item = await _repository.GetByIdAsync(id, ct);
        if (item is null)
            return Result<TierlistDetail>.Fail("Tierlist não encontrada.");

        return Result<TierlistDetail>.Ok(item);
    }

    public async Task<Result<IReadOnlyList<JogoTierResponse>>> ListJogosByTierlistIdAsync(long tierlistId, CancellationToken ct = default)
    {
        var tier = await _repository.GetByIdAsync(tierlistId, ct);
        if (tier is null)
            return Result<IReadOnlyList<JogoTierResponse>>.Fail("Tierlist não encontrada.");

        var jogos = await _repository.GetJogosByTierlistIdAsync(tierlistId, ct);
        return Result<IReadOnlyList<JogoTierResponse>>.Ok(jogos);
    }
}
