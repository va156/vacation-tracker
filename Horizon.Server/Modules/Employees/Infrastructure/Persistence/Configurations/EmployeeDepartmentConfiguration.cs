using Horizon.Server.Modules.Employees.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Server.Modules.Employees.Infrastructure.Persistence.Configurations;

public class EmployeeDepartmentConfiguration : IEntityTypeConfiguration<EmployeeDepartment>
{
    public void Configure(EntityTypeBuilder<EmployeeDepartment> builder)
    {
        builder.ToTable("EmployeeDepartments");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.FTE)
            .HasPrecision(3, 2)
            .HasDefaultValue(1.0m);

        builder.Property(e => e.IsPrimary)
            .HasDefaultValue(false);

        builder.Property(e => e.StartDate)
            .IsRequired();

        builder.HasIndex(e => new { e.EmployeeId, e.DepartmentId, e.StartDate })
            .IsUnique()
            .HasDatabaseName("IX_EmployeeDepartment_Employee_Department_StartDate");

        builder.HasIndex(e => e.DepartmentId)
            .HasDatabaseName("IX_EmployeeDepartment_DepartmentId");

        builder.HasIndex(e => e.PositionId)
            .HasDatabaseName("IX_EmployeeDepartment_PositionId");

        builder.HasOne(e => e.Employee)
            .WithMany(e => e.DepartmentAssignments)
            .HasForeignKey(e => e.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Department)
            .WithMany(d => d.EmployeeAssignments)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Position)
            .WithMany(p => p.EmployeeDepartments)
            .HasForeignKey(e => e.PositionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}