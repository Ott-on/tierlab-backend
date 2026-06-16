namespace TierLab.Application.UseCases.Tierlists.Dtos;

public sealed record TierlistSummary(
    long Id,
    string Titulo,
    string? ImageUrl
);
