using TierLab.Application.Common;
using TierLab.Application.UseCases.Jogos.Dtos;

namespace TierLab.Application.UseCases.Jogos;

public interface IJogoQueries
{
    Task<Result<IReadOnlyList<JogoResponse>>> GetAllAsync(CancellationToken ct = default);

    Task<Result<IReadOnlyList<JogoResponse>>> SearchByTituloAsync(
        string query,
        CancellationToken ct = default);
}
