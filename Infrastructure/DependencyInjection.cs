using Domain.Abstract;
using Infrastructure.DB;
using Infrastructure.MappingProfiles;
using Infrastructure.Services;
using Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DbContext = Infrastructure.DB.DbContext;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Use in-memory EF provider instead of real Postgres connection
        services.AddDbContext<DbContext>(options => options
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase("DummyData")
        );
        
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IOrderService, OrderService>();
        // Add the DB seeder to be invoked at app startup
        services.AddScoped<IDbSeeder, DbSeeder>();
        
        services.AddAutoMapper(typeof(MappingProfile));

        // // Programmatic seeding for the InMemory provider.
        // // Note: calling BuildServiceProvider during registrations is acceptable for simple
        // // seeding at startup; for more complex apps prefer an IHostedService or a
        // // dedicated seed step in Program.Main.
        // using (var provider = services.BuildServiceProvider())
        // using (var scope = provider.CreateScope())
        // {
        //     var context = scope.ServiceProvider.GetRequiredService<NotificationContext>();
        //     context.Database.EnsureCreated();
        //     
        //     if (context.Database.IsInMemory() && !context.Clients.Any())
        //     {
        //         context.Clients.AddRange(
        //             new Domain.Entities.Client { Id = 1L, Name = "John", Surname = "Doe", Email = "john.doe@example.com", PhoneNumber = "1234567890" },
        //             new Domain.Entities.Client { Id = 2L, Name = "Jane", Surname = "Smith", Email = "jane.smith@example.com", PhoneNumber = "0987654321" }
        //         );
        //         context.SaveChanges();
        //     }
        // }
    }
}