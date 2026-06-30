using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Horizon.Server.Modules.LeaveManagement.Domain.Entities;

namespace Horizon.Server.Modules.LeaveManagement.Infrastructure.Persistence.Configurations;

public class LeaveConfiguration : IEntityTypeConfiguration<Leave>
{
    public void Configure(EntityTypeBuilder<Leave> builder)
    {
        builder.ToTable("Leaves");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.DurationDays)
            .HasPrecision(5, 1)
            .IsRequired();

        builder.Property(e => e.Comment)
            .HasMaxLength(500);

        builder.Property(e => e.StartDate)
            .IsRequired();

        builder.Property(e => e.EndDate)
            .IsRequired();

        builder.HasIndex(e => e.EmployeeId)
            .HasDatabaseName("IX_Leave_EmployeeId");

        builder.HasIndex(e => e.LeaveTypeId)
            .HasDatabaseName("IX_Leave_LeaveTypeId");

        builder.HasIndex(e => e.StatusId)
            .HasDatabaseName("IX_Leave_StatusId");

        builder.HasIndex(e => e.RequestId)
            .HasDatabaseName("IX_Leave_RequestId");

        builder.HasOne(e => e.PreviousLeave)
            .WithMany(e => e.ChildLeaves)
            .HasForeignKey(e => e.PreviousLeaveId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}