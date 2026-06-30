using Horizon.Server.Modules.Employees;
using Horizon.Server.Modules.LeaveManagement;
using Horizon.Server.Modules.ApprovalWorkflow;
using Horizon.Server.Modules.References;
using Horizon.Server.Modules.Users;
using Horizon.Server.Modules.Shared.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Shared EF Core database context for the entire application.
/// Entity configurations are loaded from each module's assembly using the
/// <c>IEntityTypeConfiguration&lt;T&gt;</c> convention, keeping module-specific mapping
/// code inside its own module.
/// Global conventions (delete behaviour, decimal precision) are applied via
/// <see cref="SharedDatabaseConfiguration.ApplyGlobalConfigurations"/>.
/// </summary>
public class AppDbContext : DbContext
{
    /// <inheritdoc />
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply per-module entity configurations discovered via reflection
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Horizon.Server.Modules.Employees.ModuleRegistration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Horizon.Server.Modules.LeaveManagement.ModuleRegistration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Horizon.Server.Modules.ApprovalWorkflow.ModuleRegistration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Horizon.Server.Modules.References.ModuleRegistration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Horizon.Server.Modules.Users.ModuleRegistration).Assembly);

        // Apply shared conventions after all configurations have been registered
        SharedDatabaseConfiguration.ApplyGlobalConfigurations(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }
}
