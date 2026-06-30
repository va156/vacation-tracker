using Horizon.Server.Modules.ApprovalWorkflow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Server.Modules.Shared.Infrastructure.Persistence;

/// <summary>
/// Applies global EF Core model conventions that are shared across all modules.
/// Called from <c>AppDbContext.OnModelCreating</c> after all per-module configurations
/// have been registered.
/// </summary>
public static class SharedDatabaseConfiguration
{
    /// <summary>
    /// Applies all global conventions to the supplied <paramref name="modelBuilder"/>.
    /// </summary>
    /// <param name="modelBuilder">The EF Core model builder being configured.</param>
    public static void ApplyGlobalConfigurations(ModelBuilder modelBuilder)
    {
        ApplyDeleteBehaviorPolicy(modelBuilder);
        ApplyDecimalPrecisionConvention(modelBuilder);
    }

    /// <summary>
    /// Overrides all foreign-key delete behaviours to <see cref="DeleteBehavior.Restrict"/>
    /// to prevent accidental cascades, with a targeted exception for
    /// <see cref="ApprovalTemplate"/> → <see cref="ApprovalStage"/> (Cascade).
    /// </summary>
    private static void ApplyDeleteBehaviorPolicy(ModelBuilder modelBuilder)
    {
        foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys()))
        {
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }

        // Allow cascade deletion of stages when an approval template is removed.
        foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys())
            .Where(fk =>
                fk.PrincipalEntityType.ClrType == typeof(ApprovalTemplate) &&
                fk.Properties.Any(p => p.Name == "TemplateId")))
        {
            foreignKey.DeleteBehavior = DeleteBehavior.Cascade;
        }
    }

    /// <summary>
    /// Configures all <c>decimal</c> and <c>decimal?</c> columns to use precision 18, scale 2
    /// unless explicitly overridden in an entity configuration.
    /// </summary>
    private static void ApplyDecimalPrecisionConvention(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties()
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }
        }
    }
}
