using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Host.Configuration;

public static class ServiceConfiguration
{
    public static void ConfigureSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo 
            { 
                Title = "ClientService API", 
                Version = "v1",
                Description = "ClientService API"
            });
        });
    }
}
