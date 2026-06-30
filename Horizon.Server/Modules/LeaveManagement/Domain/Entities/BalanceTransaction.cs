using Horizon.Server.Modules.Shared.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Server.Modules.LeaveManagement.Domain.Entities;

[Index(nameof(EmployeeId))]
[Index(nameof(LeaveTypeId))]
[Index(nameof(TransactionTypeId))]
public class BalanceTransaction : BaseEntity
{
    private BalanceTransaction() { }
    public BalanceTransaction(int employeeId, int leaveTypeId, int transactionTypeId,
        decimal amount, int? requestId, int? leaveId, string? description, int createdBy)
        : base(createdBy)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive", nameof(amount));

        EmployeeId = employeeId;
        LeaveTypeId = leaveTypeId;
        TransactionTypeId = transactionTypeId;
        TransactionDate = DateTime.UtcNow;
        Amount = amount;
        RequestId = requestId;
        LeaveId = leaveId;
        Description = description;
    }

    public int EmployeeId { get; private set; }
    public int LeaveTypeId { get; private set; }
    public int TransactionTypeId { get; private set; }
    public DateTime TransactionDate { get; private set; }
    public decimal Amount { get; private set; }
    public int? LeaveId { get; private set; }
    public int? RequestId { get; private set; }
    public string? Description { get; private set; }
    public virtual Leave? Leave { get; private set; }
}