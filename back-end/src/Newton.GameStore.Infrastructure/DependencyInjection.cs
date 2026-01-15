using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newton.GameStore.Domain.Interfaces;
using Newton.GameStore.Infrastructure.Data;
using Newton.GameStore.Infrastructure.Repositories;

namespace Newton.GameStore.Infrastructure;

/// <summary>
/// Extension methods for registering Infrastructure layer services.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IVideoGameRepository, VideoGameRepository>();

        return services;
    }

    public static IServiceCollection AddInfrastructureServicesWithInMemory(
        this IServiceCollection services,
        string databaseName = "Newton.GameStoreDb")
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase(databaseName));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IVideoGameRepository, VideoGameRepository>();

        return services;
    }
}
