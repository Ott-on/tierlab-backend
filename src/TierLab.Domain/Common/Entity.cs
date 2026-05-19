namespace TierLab.Domain.Common;

public abstract class Entity<TKey> where TKey : notnull
{
    public TKey Id { get; set; } = default!;

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TKey> other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        return EqualityComparer<TKey>.Default.Equals(Id, other.Id);
    }

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Entity<TKey>? left, Entity<TKey>? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(Entity<TKey>? left, Entity<TKey>? right) => !(left == right);
}
