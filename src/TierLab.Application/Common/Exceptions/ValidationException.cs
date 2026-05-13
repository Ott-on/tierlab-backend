namespace TierLab.Application.Common.Exceptions;

/// <summary>
/// Thrown when a validation rule fails.
/// </summary>
public sealed class ValidationException : ApplicationException
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public ValidationException(IReadOnlyDictionary<string, string[]> errors)
        : base("One or more validation failures have occurred.")
    {
        Errors = errors;
    }
}
