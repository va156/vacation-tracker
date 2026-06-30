namespace Horizon.Server.Modules.Shared.Domain.Common;

/// <summary>
/// Base class for all DDD value objects. Equality is determined by the component values
/// returned from <see cref="GetEqualityComponents"/>, not by reference.
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Returns the ordered sequence of values that together define the identity of this value object.
    /// All concrete value objects must implement this method.
    /// </summary>
    protected abstract IEnumerable<object> GetEqualityComponents();

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;

        var other = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }

    /// <summary>Returns <c>true</c> when both value objects have identical components.</summary>
    public static bool operator ==(ValueObject left, ValueObject right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    /// <summary>Returns <c>true</c> when the value objects differ in at least one component.</summary>
    public static bool operator !=(ValueObject left, ValueObject right) => !(left == right);
}
