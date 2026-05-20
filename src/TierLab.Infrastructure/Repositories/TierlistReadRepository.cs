using Microsoft.EntityFrameworkCore;
using TierLab.Application.UseCases.Tierlists;
using TierLab.Application.UseCases.Tierlists.Dtos;
using TierLab.Infrastructure.Persistence;

namespace TierLab.Infrastructure.Repositories;

public sealed class TierlistReadRepository : ITierlistReadRepository
{
    private readonly AppDbContext _context;

    public TierlistReadRepository(AppDbContext context)
    {
        _context = context;
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
}
