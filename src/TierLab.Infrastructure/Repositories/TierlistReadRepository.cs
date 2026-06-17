using Microsoft.EntityFrameworkCore;
using TierLab.Application.Common;
using TierLab.Application.UseCases.Tierlists;
using TierLab.Application.UseCases.Tierlists.Dtos;
using TierLab.Application.UseCases.Jogos.Dtos;
using TierLab.Infrastructure.Persistence;

namespace TierLab.Infrastructure.Repositories;

public sealed class TierlistReadRepository : ITierlistReadRepository
{
    private readonly AppDbContext _context;

    public TierlistReadRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CursorPage<TierlistCursorItem>> GetTierlistsCursorPageAsync(
        int limit, CursorPageKey? cursor, CancellationToken ct = default)
    {
        limit = limit <= 0 ? 20 : Math.Min(limit, 100);

        var query = _context.Tierlists
            .AsNoTracking()
            .Where(t => t.Visibilidade == "public");

        if (cursor is not null)
        {
            query = query.Where(t =>
                t.CriadoEm < cursor.CreatedAt ||
                (t.CriadoEm == cursor.CreatedAt && t.Id < cursor.Id));
        }

        query = query.OrderByDescending(t => t.CriadoEm).ThenByDescending(t => t.Id);

        var items = await query
            .Select(t => new TierlistCursorItem(t.Id, t.Titulo, t.ImageUrl, t.CriadoEm))
            .Take(limit + 1)
            .ToListAsync(ct);

        var hasMore = items.Count > limit;
        if (hasMore)
            items = items.Take(limit).ToList();

        var lastKey = items.Count == 0 ? null : new CursorPageKey(items[^1].CriadoEm, items[^1].Id);
        return new CursorPage<TierlistCursorItem>(items, hasMore, lastKey);
    }

    public async Task<IReadOnlyList<TierlistSummary>> GetByGeneroAsync(
        long generoId, int limit, CancellationToken ct = default)
    {
        var query = _context.Tierlists
            .AsNoTracking()
            .Where(t => t.Visibilidade == "public")
            .Where(t => t.TierJogos.Any(tj =>
                tj.Jogo.GeneroJogos.Any(gj => gj.GeneroId == generoId)))
            .OrderByDescending(t => t.CriadoEm)
            .Select(t => new TierlistSummary(t.Id, t.Titulo, t.ImageUrl));

        if (limit > 0)
            query = query.Take(limit);

        return await query.ToListAsync(ct);
    }

    public async Task<IReadOnlyList<TierlistSummary>> GetByUsuarioAsync(Guid usuarioId, int limit, CancellationToken ct = default)
    {
        var query = _context.Tierlists
            .AsNoTracking()
            .Where(t => t.UsuarioId == usuarioId)
            .OrderByDescending(t => t.CriadoEm)
            .Select(t => new TierlistSummary(t.Id, t.Titulo, t.ImageUrl));

        if (limit > 0)
            query = query.Take(limit);

        return await query.ToListAsync(ct);
    }

    public async Task<IReadOnlyList<TierlistSummary>> SearchByTituloAsync(
        string query, int maxResults, CancellationToken ct = default)
    {
        return await _context.Tierlists
            .AsNoTracking()
            .Where(t => t.Visibilidade == "public")
            .Where(t => EF.Functions.ILike(t.Titulo, $"%{query}%"))
            .OrderByDescending(t => t.CriadoEm)
            .Take(maxResults)
            .Select(t => new TierlistSummary(t.Id, t.Titulo, t.ImageUrl))
            .ToListAsync(ct);
    }

    public async Task<TierlistSummary?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        return await _context.Tierlists
            .AsNoTracking()
            .Where(t => t.Id == id)
            .Select(t => new TierlistSummary(t.Id, t.Titulo, t.ImageUrl))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<JogoTierResponse>> GetJogosByTierlistIdAsync(long tierlistId, CancellationToken ct = default)
    {
        return await _context.TierJogos
            .AsNoTracking()
            .Where(tj => tj.TierlistId == tierlistId && tj.Tierlist.Visibilidade == "public")
            .OrderBy(tj => tj.Posicao) 
                .Select(tj => new JogoTierResponse(tj.Jogo.Id, tj.Jogo.Titulo, tj.Jogo.ImageUrl, tj.Tier, tj.Posicao))
            .ToListAsync(ct);
    }

    public async Task<CursorPage<JogoTierResponse>> GetJogosCursorPageAsync(int limit, CursorPageKey? cursor, long tierlistId, CancellationToken ct = default)
    {
        limit = limit <= 0 ? 20 : Math.Min(limit, 100);

        var query = _context.TierJogos
            .AsNoTracking()
            .Where(tj => tj.TierlistId == tierlistId && tj.Tierlist.Visibilidade == "public");

        if (cursor is not null)
        {
            query = query.Where(tj =>
                tj.AdicionadoEm < cursor.CreatedAt ||
                (tj.AdicionadoEm == cursor.CreatedAt && tj.JogoId < cursor.Id));
        }

        query = query.OrderByDescending(tj => tj.AdicionadoEm).ThenByDescending(tj => tj.JogoId);

        var items = await query
                .Select(tj => new JogoTierResponse(tj.Jogo.Id, tj.Jogo.Titulo, tj.Jogo.ImageUrl, tj.Tier, tj.Posicao))
            .Take(limit + 1)
            .ToListAsync(ct);

        var hasMore = items.Count > limit;
        if (hasMore)
            items = items.Take(limit).ToList();

        // Need last key: take last returned entity's AdicionadoEm and JogoId
        CursorPageKey? lastKey = null;
        if (items.Count > 0)
        {
            // determine last item's id without using '^' operator inside expression trees
            var lastItemId = items[items.Count - 1].Id;

            var lastItem = await _context.TierJogos
                .AsNoTracking()
                .Where(tj => tj.TierlistId == tierlistId && tj.JogoId == lastItemId)
                .Select(tj => new { tj.AdicionadoEm, tj.JogoId })
                .OrderByDescending(tj => tj.AdicionadoEm)
                .ThenByDescending(tj => tj.JogoId)
                .FirstOrDefaultAsync(ct);

            if (lastItem is not null)
                lastKey = new CursorPageKey(lastItem.AdicionadoEm, lastItem.JogoId);
        }

        return new CursorPage<JogoTierResponse>(items, hasMore, lastKey);
    }
}
