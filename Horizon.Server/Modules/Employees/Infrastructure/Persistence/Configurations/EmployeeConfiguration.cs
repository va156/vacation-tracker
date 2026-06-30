using Horizon.Server.Modules.Employees.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Server.Modules.Employees.Infrastructure.Persistence.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.EmployeeNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.HireDate)
            .IsRequired();

        builder.HasIndex(e => e.EmployeeNumber)
            .IsUnique()
            .HasDatabaseName("IX_Employee_EmployeeNumber");

        builder.HasOne(e => e.Manager)
            .WithMany(e => e.Subordinates)
            .HasForeignKey(e => e.ManagerId)
            .OnDelete(DeleteBehavior.Restrict); 
    }
}