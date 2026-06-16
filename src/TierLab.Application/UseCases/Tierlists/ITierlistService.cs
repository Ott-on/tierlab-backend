using TierLab.Application.Common;
using TierLab.Application.UseCases.Tierlists.Dtos;
using TierLab.Application.UseCases.Tierlists.Requests;

namespace TierLab.Application.UseCases.Tierlists;

public interface ITierlistService
{
    Task<Result<TierlistSummary>> CreateAsync(Guid usuarioId, CreateTierlistRequest request, CancellationToken ct = default);
    Task<Result> AddJogoAsync(long tierlistId, AddTierlistJogoRequest request, CancellationToken ct = default);
    Task<Result> RemoveJogoAsync(long tierlistId, long jogoId, CancellationToken ct = default);
}
