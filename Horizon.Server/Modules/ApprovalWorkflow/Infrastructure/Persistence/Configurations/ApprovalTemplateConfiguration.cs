using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Horizon.Server.Modules.ApprovalWorkflow.Domain.Entities;

namespace Horizon.Server.Modules.ApprovalWorkflow.Infrastructure.Persistence.Configurations;

public class ApprovalTemplateConfiguration : IEntityTypeConfiguration<ApprovalTemplate>
{
    public void Configure(EntityTypeBuilder<ApprovalTemplate> builder)
    {
        builder.ToTable("ApprovalTemplates");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(500);

        builder.Property(e => e.SortOrder)
            .HasDefaultValue(0);

        // Отношения (каскадное удаление этапов)
        builder.HasMany(e => e.Stages)
            .WithOne(e => e.Template)
            .HasForeignKey(e => e.TemplateId)
            .OnDelete(DeleteBehavior.Cascade); //переопределяет Restrict из Shared
    }
}