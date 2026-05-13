using TierLab.Domain.Interfaces;
using TierLab.Infrastructure.Persistence;

namespace TierLab.Infrastructure.Persistence;

/// <summary>
/// Unit of Work implementation wrapping EF Core SaveChanges.
/// </summary>
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);

    public void Dispose() => _context.Dispose();
}
