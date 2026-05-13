namespace TierLab.Domain.Interfaces;

/// <summary>
/// Abstraction for coordinating transactional persistence.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
