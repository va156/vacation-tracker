using System.Text.RegularExpressions;
using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Domain.ValueObjects;

/// <summary>
/// Value object that encapsulates and validates a phone number.
/// Accepts strings in the range 10–15 digits with an optional leading <c>+</c>.
/// </summary>
public partial class PhoneNumber : ValueObject
{
    private static readonly Regex PhoneRegex = MyRegex();

    /// <summary>Gets the validated phone number string (preserves the original formatting).</summary>
    public string Value { get; }

    /// <summary>
    /// Creates a new <see cref="PhoneNumber"/> value object.
    /// </summary>
    /// <param name="value">Raw phone string to validate and wrap.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is null/empty or does not match the phone pattern.
    /// </exception>
    public PhoneNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Phone number cannot be empty");

        if (!PhoneRegex.IsMatch(value))
            throw new ArgumentException("Invalid phone number format");

        Value = value;
    }

    [GeneratedRegex(@"^\+?[0-9]{10,15}$", RegexOptions.Compiled)]
    private static partial Regex MyRegex();

    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>Returns the phone number string.</summary>
    public override string ToString() => Value;
}
