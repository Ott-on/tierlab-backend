namespace TierLab.Application.UseCases.Jogos.Dtos;

public sealed record JogoTierResponse(
    long Id,
    string Titulo,
    string? ImageUrl,
    string? Tier = null,
    int? Posicao = 0
);
