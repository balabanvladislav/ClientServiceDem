using Domain.Configs;

namespace Host.Configuration;

public static class DependencyInjection
{
    public static void AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettingsConfig>(configuration.GetSection(JwtSettingsConfig.Key));
    }
}