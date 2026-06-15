using Microsoft.EntityFrameworkCore;
using TierLab.Application.UseCases.Jogos;
using TierLab.Application.UseCases.Jogos.Dtos;
using TierLab.Infrastructure.Persistence;

namespace TierLab.Infrastructure.Repositories;

public sealed class JogoReadRepository : IJogoReadRepository
{
    private readonly AppDbContext _context;

    public JogoReadRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<JogoResponse>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Jogos
            .AsNoTracking()
            .OrderBy(j => j.Titulo)
            .Select(j => new JogoResponse(j.Id, j.Titulo, j.ImageUrl, j.AnoLancamento))
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<JogoResponse>> SearchByTituloAsync(
        string query,
        int maxResults,
        CancellationToken ct = default)
    {
        return await _context.Jogos
            .AsNoTracking()
            .Where(j => EF.Functions.ILike(j.Titulo, $"%{query}%"))
            .OrderBy(j => j.Titulo)
            .Take(maxResults)
            .Select(j => new JogoResponse(j.Id, j.Titulo, j.ImageUrl, j.AnoLancamento))
            .ToListAsync(ct);
    }
}
