using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VideoGameCatalogue.Domain.Interfaces;
using VideoGameCatalogue.Infrastructure.Data;
using VideoGameCatalogue.Infrastructure.Repositories;

namespace VideoGameCatalogue.Infrastructure;

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
        string databaseName = "VideoGameCatalogueDb")
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase(databaseName));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IVideoGameRepository, VideoGameRepository>();

        return services;
    }
}
