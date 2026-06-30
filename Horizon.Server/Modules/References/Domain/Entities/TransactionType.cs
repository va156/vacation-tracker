using Horizon.Server.Modules.Shared.Domain.Common;

namespace Horizon.Server.Modules.References.Domain.Entities;

/// <summary>
/// Reference entity that classifies a balance transaction as a credit (<c>+</c>) or debit (<c>-</c>).
/// The <see cref="Sign"/> property drives the balance adjustment calculation so that
/// the application code remains free of magic strings.
/// </summary>
public class TransactionType : BaseEntity, IReferenceEntity
{
    /// <summary>EF Core parameterless constructor — not for direct use.</summary>
    private TransactionType() { }

    /// <summary>
    /// Creates a new transaction type.
    /// </summary>
    /// <param name="code">Unique business code (e.g. "ACCRUAL", "DEDUCTION").</param>
    /// <param name="name">Display name.</param>
    /// <param name="sign"><c>"+"</c> for credits (balance increases) or <c>"-"</c> for debits (balance decreases).</param>
    /// <param name="sortOrder">Display sort order.</param>
    /// <param name="createdBy">ID of the user creating this record.</param>
    public TransactionType(string code, string name, string sign, int sortOrder, int createdBy) : base(createdBy)
    {
        Code = code;
        Name = name;
        Sign = sign;
        SortOrder = sortOrder;
    }

    /// <summary>Gets the unique business code.</summary>
    public string Code { get; private set; } = null!;

    /// <summary>Gets the display name.</summary>
    public string Name { get; private set; } = null!;

    /// <summary>
    /// Gets the arithmetic sign of this transaction type:
    /// <c>"+"</c> means the balance increases (accrual, correction), <c>"-"</c> means it decreases (deduction, cancellation).
    /// </summary>
    public string Sign { get; private set; } = null!;

    /// <summary>Gets the display sort order.</summary>
    public int SortOrder { get; private set; }
}
