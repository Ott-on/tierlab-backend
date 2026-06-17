namespace TierLab.Application.UseCases.Tierlists.Requests;

public sealed record TierlistsCursorRequest(int Limit = 20, string? Cursor = null);
