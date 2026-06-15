using TierLab.Application.Common;
using TierLab.Application.UseCases.Jogos.Dtos;

namespace TierLab.Application.UseCases.Jogos;

public sealed class JogoQueries : IJogoQueries
{
    private readonly IJogoReadRepository _repository;

    public JogoQueries(IJogoReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IReadOnlyList<JogoResponse>>> GetAllAsync(
        CancellationToken ct = default)
    {
        var jogos = await _repository.GetAllAsync(ct);
        return Result<IReadOnlyList<JogoResponse>>.Ok(jogos);
    }

    public async Task<Result<IReadOnlyList<JogoResponse>>> SearchByTituloAsync(
        string query,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return Result<IReadOnlyList<JogoResponse>>.Ok([]);
        }

        var jogos = await _repository.SearchByTituloAsync(query.Trim(), maxResults: 10, ct);
        return Result<IReadOnlyList<JogoResponse>>.Ok(jogos);
    }
}
