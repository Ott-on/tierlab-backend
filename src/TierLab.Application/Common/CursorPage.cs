namespace TierLab.Application.Common;

public sealed record CursorPage<T>(IReadOnlyList<T> Items, bool HasMore, CursorPageKey? LastKey);
