using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Server.Modules.ApprovalWorkflow.Infrastructure.Persistence.Configurations;

public class ApprovalStageConfiguration : IEntityTypeConfiguration<ApprovalStage>
{
    public void Configure(EntityTypeBuilder<ApprovalStage> builder)
    {
        builder.ToTable("ApprovalStages");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.StageName)
            .HasMaxLength(200);

        builder.Property(e => e.IsRequired)
            .HasDefaultValue(true);

        builder.HasIndex(e => e.TemplateId)
            .HasDatabaseName("IX_ApprovalStage_TemplateId");

        builder.HasIndex(e => e.RoleId)
            .HasDatabaseName("IX_ApprovalStage_RoleId");

        builder.HasIndex(e => e.DepartmentId)
            .HasDatabaseName("IX_ApprovalStage_DepartmentId");

        builder.HasIndex(e => e.PositionId)
            .HasDatabaseName("IX_ApprovalStage_PositionId");

        builder.HasOne(e => e.Template)
            .WithMany(t => t.Stages)
            .HasForeignKey(e => e.TemplateId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}