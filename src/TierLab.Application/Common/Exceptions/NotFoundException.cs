namespace TierLab.Application.Common.Exceptions;

/// <summary>
/// Thrown when a requested resource is not found.
/// </summary>
public sealed class NotFoundException : ApplicationException
{
    public NotFoundException(string entityName, object key)
        : base($"Entity \"{entityName}\" with key ({key}) was not found.") { }
}
