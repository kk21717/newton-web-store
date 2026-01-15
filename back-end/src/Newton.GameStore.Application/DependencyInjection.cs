using Microsoft.Extensions.DependencyInjection;
using Newton.GameStore.Application.Interfaces;
using Newton.GameStore.Application.Services;

namespace Newton.GameStore.Application;

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
