using System.Text.Json.Serialization;
using Features.Endpoints.Client;
using Features.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Features;

public static class DependencyInjection
{
    public static void AddFeatures(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterEndpointsFromAssemblyContaining<GetClientsEndpoint>();

        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining<GetClientsEndpoint>();
        });
        
        
        services.AddValidatorsFromAssemblyContaining<GetClientsEndpoint>();
        
        
        services.Configure<JsonOptions>(opt =>
        {
            opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            opt.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });
    }

    public static void MapFeatures(this WebApplication app)
    {
        app.MapEndpoints();
    }
}