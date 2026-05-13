namespace TierLab.Application.Common.Exceptions;

/// <summary>
/// Thrown when a business rule is violated.
/// </summary>
public sealed class BusinessRuleException : ApplicationException
{
    public BusinessRuleException(string message) : base(message) { }
}
