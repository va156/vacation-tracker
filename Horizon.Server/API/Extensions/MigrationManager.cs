using Microsoft.EntityFrameworkCore;

namespace Horizon.Server.Extensions;

public static class MigrationManager
{
    public static IHost MigrateDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<AppDbContext>();
            context.Database.Migrate();
            Console.WriteLine("Database migrated successfully");
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating the database");

            // Останавливаем приложение, если миграции не применились
            throw;
        }

        return host;
    }
}