namespace TierLab.Application.UseCases.Jogos.Dtos;

public sealed record JogoResponse(
    long Id,
    string Titulo,
    string? ImageUrl
);
