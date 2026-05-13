namespace TierLab.Application.Common;

/// <summary>
/// Standardized API response wrapper.
/// Ensures all endpoints return a consistent shape.
/// </summary>
public sealed class Result<T>
{
    public bool Success { get; private init; }
    public T? Data { get; private init; }
    public string? Message { get; private init; }
    public IReadOnlyList<string> Errors { get; private init; } = [];

    public static Result<T> Ok(T data, string? message = null) => new()
    {
        Success = true,
        Data = data,
        Message = message
    };

    public static Result<T> Fail(string error) => new()
    {
        Success = false,
        Errors = [error]
    };

    public static Result<T> Fail(IEnumerable<string> errors) => new()
    {
        Success = false,
        Errors = errors.ToList().AsReadOnly()
    };
}

/// <summary>
/// Non-generic result for operations that don't return data.
/// </summary>
public sealed class Result
{
    public bool Success { get; private init; }
    public string? Message { get; private init; }
    public IReadOnlyList<string> Errors { get; private init; } = [];

    public static Result Ok(string? message = null) => new()
    {
        Success = true,
        Message = message
    };

    public static Result Fail(string error) => new()
    {
        Success = false,
        Errors = [error]
    };

    public static Result Fail(IEnumerable<string> errors) => new()
    {
        Success = false,
        Errors = errors.ToList().AsReadOnly()
    };
}
