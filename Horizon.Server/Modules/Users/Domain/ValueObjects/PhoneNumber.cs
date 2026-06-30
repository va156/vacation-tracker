using System.Text.RegularExpressions;
using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Domain.ValueObjects;

public partial class PhoneNumber : ValueObject
{
    private static readonly Regex PhoneRegex = MyRegex();

    public string Value { get; }

    public PhoneNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Телефон не может быть пустым");

        if (!PhoneRegex.IsMatch(value))
            throw new ArgumentException("Неверный формат телефона");

        Value = value;
    }

    [GeneratedRegex(@"^\+?[0-9]{10,15}$", RegexOptions.Compiled)]
    private static partial Regex MyRegex();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
