namespace TierLab.Application.UseCases.Tierlists.Dtos;

public sealed record TierlistCursorItem(
    long Id,
    string Titulo,
    string? ImageUrl,
    DateTime CriadoEm);
