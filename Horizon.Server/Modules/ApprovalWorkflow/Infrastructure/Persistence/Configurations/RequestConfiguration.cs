using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Horizon.Server.Modules.ApprovalWorkflow.Domain.Entities;

namespace Horizon.Server.Modules.ApprovalWorkflow.Infrastructure.Persistence.Configurations;

public class RequestConfiguration : IEntityTypeConfiguration<Request>
{
    public void Configure(EntityTypeBuilder<Request> builder)
    {
        builder.ToTable("Requests");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.RequestNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Comment)
            .HasMaxLength(1000);

        builder.HasIndex(e => e.RequestNumber)
            .IsUnique()
            .HasDatabaseName("IX_Request_RequestNumber");

        builder.HasIndex(e => e.EmployeeId)
            .HasDatabaseName("IX_Request_EmployeeId");

        builder.HasIndex(e => e.OperationTypeId)
            .HasDatabaseName("IX_Request_OperationTypeId");

        builder.HasIndex(e => e.StatusId)
            .HasDatabaseName("IX_Request_StatusId");

        builder.HasIndex(e => e.DepartmentId)
            .HasDatabaseName("IX_Request_DepartmentId");

        builder.HasIndex(e => e.ApprovalTemplateId)
            .HasDatabaseName("IX_Request_ApprovalTemplateId");

        builder.HasIndex(e => new { e.EmployeeId, e.CreatedAt })
            .HasDatabaseName("IX_Request_EmployeeId_CreatedAt");

        builder.HasOne(e => e.ApprovalTemplate)
            .WithMany()
            .HasForeignKey(e => e.ApprovalTemplateId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}