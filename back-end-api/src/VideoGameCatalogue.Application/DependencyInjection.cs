using Microsoft.Extensions.DependencyInjection;
using VideoGameCatalogue.Application.Interfaces;
using VideoGameCatalogue.Application.Services;

namespace VideoGameCatalogue.Application;

/// <summary>
/// Extension methods for registering Application layer services.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IVideoGameService, VideoGameService>();
        return services;
    }
}
