using Horizon.Server.Modules.ApprovalWorkflow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Server.Modules.Shared.Infrastructure.Persistence;

public static class SharedDatabaseConfiguration
{
    public static void ApplyGlobalConfigurations(ModelBuilder modelBuilder)
    {
        ApplyDeleteBehaviorPolicy(modelBuilder);
        ApplyDecimalPrecisionConvention(modelBuilder);
    }

    private static void ApplyDeleteBehaviorPolicy(ModelBuilder modelBuilder)
    {
        // Все внешние ключи - Restrict (защита от случайного удаления)
        foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys()))
        {
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }

        // Исключение: каскадное удаление этапов при удалении шаблона
        foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys())
            .Where(fk =>
                fk.PrincipalEntityType.ClrType == typeof(ApprovalTemplate) &&
                fk.Properties.Any(p => p.Name == "TemplateId")))
        {
            foreignKey.DeleteBehavior = DeleteBehavior.Cascade;
        }
    }

    private static void ApplyDecimalPrecisionConvention(ModelBuilder modelBuilder)
    {
        // Все decimal поля по умолчанию имеют точность 18,2
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