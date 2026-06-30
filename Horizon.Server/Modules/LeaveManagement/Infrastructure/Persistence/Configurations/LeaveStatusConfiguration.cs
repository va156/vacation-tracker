using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Horizon.Server.Modules.LeaveManagement.Domain.Entities;

namespace Horizon.Server.Modules.LeaveManagement.Infrastructure.Persistence.Configurations;

public class LeaveStatusConfiguration : IEntityTypeConfiguration<LeaveStatus>
{
    public void Configure(EntityTypeBuilder<LeaveStatus> builder)
    {
        builder.ToTable("LeaveStatuses");
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