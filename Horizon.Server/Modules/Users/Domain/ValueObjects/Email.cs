using System.Text.RegularExpressions;
using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Domain.ValueObjects;

public partial class Email : ValueObject
{
    private static readonly Regex EmailRegex = MyRegex();

    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email не может быть пустым");

        if (!EmailRegex.IsMatch(value))
            throw new ArgumentException("Неверный формат email");

        Value = value;
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled)]
    private static partial Regex MyRegex();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value.ToLowerInvariant();
    }

    public override string ToString() => Value;
}
