using Microsoft.EntityFrameworkCore;
using TierLab.Domain.Entities;
using TierLab.Domain.Interfaces;
using TierLab.Infrastructure.Persistence;

namespace TierLab.Infrastructure.Repositories;

public sealed class TierlistRepository : ITierlistRepository
{
    private readonly AppDbContext _context;

    public TierlistRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Tierlist> AddAsync(Tierlist tierlist, CancellationToken cancellationToken = default)
    {
        var entry = await _context.Tierlists.AddAsync(tierlist, cancellationToken);
        return entry.Entity;
    }

    public async Task<bool> TierlistExistsAsync(long tierlistId, CancellationToken cancellationToken = default)
        => await _context.Tierlists.AnyAsync(t => t.Id == tierlistId, cancellationToken);

    public async Task<bool> JogoExistsAsync(long jogoId, CancellationToken cancellationToken = default)
        => await _context.Jogos.AnyAsync(j => j.Id == jogoId, cancellationToken);

    public async Task<TierJogo?> GetTierJogoAsync(long tierlistId, long jogoId, CancellationToken cancellationToken = default)
        => await _context.TierJogos.FindAsync(new object[] { tierlistId, jogoId }, cancellationToken);

    public async Task AddTierJogoAsync(TierJogo tierJogo, CancellationToken cancellationToken = default)
    {
        await _context.TierJogos.AddAsync(tierJogo, cancellationToken);
    }

    public Task RemoveTierJogoAsync(TierJogo tierJogo, CancellationToken cancellationToken = default)
    {
        _context.TierJogos.Remove(tierJogo);
        return Task.CompletedTask;
    }
}
