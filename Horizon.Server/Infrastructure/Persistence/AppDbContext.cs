using Horizon.Server.Modules.Employees;
using Horizon.Server.Modules.LeaveManagement;
using Horizon.Server.Modules.ApprovalWorkflow;
using Horizon.Server.Modules.References;
using Horizon.Server.Modules.Users;
using Horizon.Server.Modules.Shared.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Horizon.Server.Modules.Employees.ModuleRegistration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Horizon.Server.Modules.LeaveManagement.ModuleRegistration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Horizon.Server.Modules.ApprovalWorkflow.ModuleRegistration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Horizon.Server.Modules.References.ModuleRegistration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Horizon.Server.Modules.Users.ModuleRegistration).Assembly);

        SharedDatabaseConfiguration.ApplyGlobalConfigurations(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }
}