namespace MyBudgets.Common.Model;

public abstract class Entity<TId> where TId: struct, IEquatable<TId>
{
    protected Entity()
    {
    }

    protected Entity(TId id) => Id = id;
    
    public TId Id { get; protected set; }
    public bool IsTransient => Id.Equals(default);

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TId> otherEntity) return false;
        
        if (ReferenceEquals(this, otherEntity)) return true;
        
        if (otherEntity.IsTransient || IsTransient) return false;
        return Id.Equals(otherEntity.Id);
        
    }

    public override int GetHashCode() => Id.GetHashCode();
    
    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
    {
        return left is null ? right is null : left.Equals(right);
    }

    public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
    {
        return !(left == right);
    }
}