using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Horizon.Server.Modules.ApprovalWorkflow.Domain.Entities;

namespace Horizon.Server.Modules.ApprovalWorkflow.Infrastructure.Persistence.Configurations;

public class ApprovalHistoryConfiguration : IEntityTypeConfiguration<ApprovalHistory>
{
    public void Configure(EntityTypeBuilder<ApprovalHistory> builder)
    {
        builder.ToTable("ApprovalHistories");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Comment)
            .HasMaxLength(500);

        builder.Property(e => e.DecisionDate)
            .IsRequired();

        builder.HasIndex(e => e.RequestId)
            .HasDatabaseName("IX_ApprovalHistory_RequestId");

        builder.HasIndex(e => e.ApproverId)
            .HasDatabaseName("IX_ApprovalHistory_ApproverId");

        builder.HasIndex(e => e.DecisionId)
            .HasDatabaseName("IX_ApprovalHistory_DecisionId");

        builder.HasOne(e => e.Decision)
            .WithMany()
            .HasForeignKey(e => e.DecisionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}