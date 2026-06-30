using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Horizon.Server.Modules.LeaveManagement.Domain.Entities;

namespace Horizon.Server.Modules.LeaveManagement.Infrastructure.Persistence.Configurations;

public class LeaveBalanceConfiguration : IEntityTypeConfiguration<LeaveBalance>
{
    public void Configure(EntityTypeBuilder<LeaveBalance> builder)
    {
        builder.ToTable("LeaveBalances");
        builder.HasKey(e => e.Id);

        // Свойства
        builder.Property(e => e.Entitled)
            .HasPrecision(5, 1)
            .HasDefaultValue(0);

        builder.Property(e => e.Used)
            .HasPrecision(5, 1)
            .HasDefaultValue(0);

        builder.Property(e => e.Planned)
            .HasPrecision(5, 1)
            .HasDefaultValue(0);

        builder.Property(e => e.Year)
            .IsRequired();

        builder.Property(e => e.CalculatedAt)
            .IsRequired();

        builder.HasIndex(e => new { e.EmployeeId, e.LeaveTypeId, e.Year })
            .IsUnique()
            .HasDatabaseName("IX_LeaveBalance_Employee_LeaveType_Year");

        builder.HasOne(e => e.LeaveType)
            .WithMany()
            .HasForeignKey(e => e.LeaveTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}