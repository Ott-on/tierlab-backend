namespace TierLab.Application.UseCases.Tierlists.Dtos;

public sealed record TierlistDetail(
    long Id,
    string Titulo,
    string? ImageUrl,
    string? Descricao
);
