using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesSuite.Domain.Interfaces;
using SalesSuite.Infrastructure.Data;
using SalesSuite.Infrastructure.Repositories;

namespace SalesSuite.Infrastructure;

/// <summary>
/// Extension methods for dependency injection
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Add infrastructure services to the DI container
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        string connectionString)
    {
        // Register DbContext
        services.AddDbContext<SalesDbContext>(options =>
            options.UseSqlServer(connectionString,
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(SalesDbContext).Assembly.FullName);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelaySeconds: 30, errorNumbersToAdd: null);
                }));

        // Register Unit of Work and Generic Repository
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        return services;
    }
}
