using Features;
using Host.Configuration;
using Host.Middleware;
using Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// logging
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

// Health Checks
builder.Services
    .AddEndpointsApiExplorer()
    .AddHealthChecks();

// Swagger
ServiceConfiguration.ConfigureSwagger(builder.Services);

// JWT Authentication, can be deleted if not used
ServiceConfiguration.ConfigureAuthenticationAndAuthorization(builder.Services, builder.Configuration);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddFeatures(builder.Configuration);
builder.Services.AddConfigurations(builder.Configuration);

var app = builder.Build();

// Run DB seeding here (scoped)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>().CreateLogger("Program");

    try
    {
        var seeder = services.GetService<Infrastructure.Seeding.IDbSeeder>();
        if (seeder is not null)
        {
            logger.LogInformation("Running database seeding...");
            await seeder.SeedAsync();
            logger.LogInformation("Database seeding completed.");
        }
        else
        {
            logger.LogDebug("No IDbSeeder registered; skipping seeding.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while seeding the database.");
        throw;
    }
}

app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapFeatures();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger(c =>
{
    c.RouteTemplate = "swagger/{documentName}/swagger.json";
});

app.UseSwaggerUI(c =>
{
    c.RoutePrefix = "swagger";
    c.DocumentTitle = "ClientService API";
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    c.DefaultModelsExpandDepth(1);
    c.EnablePersistAuthorization();
});

app.Run();
