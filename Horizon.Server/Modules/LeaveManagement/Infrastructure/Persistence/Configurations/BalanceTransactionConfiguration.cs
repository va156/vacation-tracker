using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Horizon.Server.Modules.LeaveManagement.Domain.Entities;

namespace Horizon.Server.Modules.LeaveManagement.Infrastructure.Persistence.Configurations;

public class BalanceTransactionConfiguration : IEntityTypeConfiguration<BalanceTransaction>
{
    public void Configure(EntityTypeBuilder<BalanceTransaction> builder)
    {
        builder.ToTable("BalanceTransactions");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Amount)
            .HasPrecision(5, 1)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(500);

        builder.Property(e => e.TransactionDate)
            .IsRequired();

        builder.HasIndex(e => e.EmployeeId)
            .HasDatabaseName("IX_BalanceTransaction_EmployeeId");

        builder.HasIndex(e => e.LeaveTypeId)
            .HasDatabaseName("IX_BalanceTransaction_LeaveTypeId");

        builder.HasIndex(e => e.TransactionTypeId)
            .HasDatabaseName("IX_BalanceTransaction_TransactionTypeId");

        builder.HasIndex(e => e.LeaveId)
            .HasDatabaseName("IX_BalanceTransaction_LeaveId");

        builder.HasIndex(e => e.RequestId)
            .HasDatabaseName("IX_BalanceTransaction_RequestId");

    }
}