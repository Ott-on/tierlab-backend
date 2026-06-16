using TierLab.Application.UseCases.Jogos.Dtos;

namespace TierLab.Application.UseCases.Jogos;

public interface IJogoReadRepository
{
    Task<IReadOnlyList<JogoResponse>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<JogoResponse>> SearchByTituloAsync(string query, int maxResults, CancellationToken ct = default);
}
