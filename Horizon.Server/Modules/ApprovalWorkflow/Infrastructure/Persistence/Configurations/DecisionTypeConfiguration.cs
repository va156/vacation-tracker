using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Horizon.Server.Modules.ApprovalWorkflow.Domain.Entities;

namespace Horizon.Server.Modules.ApprovalWorkflow.Infrastructure.Persistence.Configurations;

public class DecisionTypeConfiguration : IEntityTypeConfiguration<DecisionType>
{
    public void Configure(EntityTypeBuilder<DecisionType> builder)
    {
        builder.ToTable("DecisionTypes");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.SortOrder)
            .HasDefaultValue(0);
    }
}