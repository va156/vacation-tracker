using System.Text.RegularExpressions;
using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Domain.ValueObjects;

/// <summary>
/// Value object that encapsulates and validates an email address.
/// Two <see cref="Email"/> instances are considered equal when their normalised
/// (lower-case) values match, ensuring case-insensitive comparison semantics.
/// </summary>
public partial class Email : ValueObject
{
    private static readonly Regex EmailRegex = MyRegex();

    /// <summary>Gets the validated, original-case email string.</summary>
    public string Value { get; }

    /// <summary>
    /// Creates a new <see cref="Email"/> value object.
    /// </summary>
    /// <param name="value">Raw email string to validate and wrap.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is null/empty or does not match the email pattern.
    /// </exception>
    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty");

        if (!EmailRegex.IsMatch(value))
            throw new ArgumentException("Invalid email format");

        Value = value;
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled)]
    private static partial Regex MyRegex();

    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value.ToLowerInvariant();
    }

    /// <summary>Returns the email string.</summary>
    public override string ToString() => Value;
}
