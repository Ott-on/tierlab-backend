using TierLab.Application.Common;
using TierLab.Application.UseCases.Tierlists.Dtos;
using TierLab.Application.UseCases.Tierlists.Requests;
using TierLab.Domain.Entities;
using TierLab.Domain.Interfaces;

namespace TierLab.Application.UseCases.Tierlists;

public sealed class TierlistService : ITierlistService
{
    private readonly ITierlistRepository _tierlistRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TierlistService(
        ITierlistRepository tierlistRepository,
        IUnitOfWork unitOfWork)
    {
        _tierlistRepository = tierlistRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TierlistSummary>> CreateAsync(
        Guid usuarioId,
        CreateTierlistRequest request,
        CancellationToken ct = default)
    {
        var tierlist = new Tierlist
        {
            UsuarioId = usuarioId,
            Titulo = request.Titulo.Trim(),
            Descricao = string.IsNullOrWhiteSpace(request.Descricao) ? null : request.Descricao.Trim(),
            ImageUrl = string.IsNullOrWhiteSpace(request.ImageUrl) ? null : request.ImageUrl.Trim(),
            Visibilidade = string.IsNullOrWhiteSpace(request.Visibilidade) ? "public" : request.Visibilidade.Trim().ToLowerInvariant()
        };

        await _tierlistRepository.AddAsync(tierlist, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        var summary = new TierlistSummary(tierlist.Id, tierlist.Titulo, tierlist.ImageUrl);
        return Result<TierlistSummary>.Ok(summary);
    }

    public async Task<Result> AddJogoAsync(long tierlistId, AddTierlistJogoRequest request, CancellationToken ct = default)
    {
        if (!await _tierlistRepository.TierlistExistsAsync(tierlistId, ct))
            return Result.Fail("Tierlist não encontrada.");

        if (!await _tierlistRepository.JogoExistsAsync(request.JogoId, ct))
            return Result.Fail("Jogo não encontrado.");

        var existing = await _tierlistRepository.GetTierJogoAsync(tierlistId, request.JogoId, ct);
        if (existing is not null)
            return Result.Fail("Jogo já está adicionado a essa tierlist.");

        var tierJogo = new TierJogo
        {
            TierlistId = tierlistId,
            JogoId = request.JogoId,
            Tier = string.IsNullOrWhiteSpace(request.Tier) ? "default" : request.Tier.Trim(),
            Posicao = request.Posicao ?? 0
        };

        await _tierlistRepository.AddTierJogoAsync(tierJogo, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok();
    }

    public async Task<Result> RemoveJogoAsync(long tierlistId, long jogoId, CancellationToken ct = default)
    {
        var existing = await _tierlistRepository.GetTierJogoAsync(tierlistId, jogoId, ct);
        if (existing is null)
            return Result.Fail("Jogo não está presente nessa tierlist.");

        await _tierlistRepository.RemoveTierJogoAsync(existing, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok();
    }
}
