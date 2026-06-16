using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TierLab.Domain.Common;
using TierLab.Domain.Interfaces;

namespace TierLab.Infrastructure.Persistence.Repositories;

public class RepositoryBase<T, TKey> : IRepository<T, TKey>
    where T : Entity<TKey>
    where TKey : notnull
{
    protected readonly AppDbContext Context;
    protected readonly DbSet<T> DbSet;

    public RepositoryBase(AppDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        => await DbSet.FindAsync([id], cancellationToken);

    public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        => await DbSet.AsNoTracking().ToListAsync(cancellationToken);

    public virtual async Task<IReadOnlyList<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
        => await DbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);

    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbSet.Update(entity);
        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public virtual async Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default)
        => await DbSet.AnyAsync(e => EqualityComparer<TKey>.Default.Equals(e.Id, id), cancellationToken);
}
