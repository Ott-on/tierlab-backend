using TierLab.Domain.Entities;

namespace TierLab.Domain.Interfaces;

public interface ITierlistRepository
{
    Task<Tierlist> AddAsync(Tierlist tierlist, CancellationToken cancellationToken = default);
    Task<bool> TierlistExistsAsync(long tierlistId, CancellationToken cancellationToken = default);
    Task<bool> JogoExistsAsync(long jogoId, CancellationToken cancellationToken = default);
    Task<TierJogo?> GetTierJogoAsync(long tierlistId, long jogoId, CancellationToken cancellationToken = default);
    Task AddTierJogoAsync(TierJogo tierJogo, CancellationToken cancellationToken = default);
    Task RemoveTierJogoAsync(TierJogo tierJogo, CancellationToken cancellationToken = default);
}
