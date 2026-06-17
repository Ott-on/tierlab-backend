namespace TierLab.Application.Common;

public sealed record CursorPageResponse<T>(IReadOnlyList<T> Items, string? NextCursor, bool HasMore);
