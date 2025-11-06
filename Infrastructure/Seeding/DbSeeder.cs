using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DbContext = Infrastructure.DB.DbContext;

namespace Infrastructure.Seeding;

public interface IDbSeeder
{
    Task SeedAsync(CancellationToken cancellationToken = default);
}

public class DbSeeder : IDbSeeder
{
    private readonly DbContext _context;
    private readonly ILogger<DbSeeder> _logger;

    public DbSeeder(DbContext context, ILogger<DbSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        _context.Database.EnsureCreated();
        
        await SeedClientsAsync(cancellationToken);
        await SeedOrdersAsync(cancellationToken);
    }

    private async Task SeedClientsAsync(CancellationToken cancellationToken)
    {
        if (await _context.Clients.AnyAsync(cancellationToken))
        {
            _logger.LogDebug("Clients already seeded.");
            return;
        }

        var seed = new List<Domain.Entities.Client>
        {
            new() { Name = "John", Surname = "Doe", Email = "john.doe@example.com", PhoneNumber = "1234567890" },
            new() { Name = "Jane", Surname = "Smith", Email = "jane.smith@example.com", PhoneNumber = "0987654321" },
            new() { Name = "Alice", Surname = "Brown", Email = "alice.brown@example.com", PhoneNumber = "2015550101" },
            new() { Name = "Bob", Surname = "Johnson", Email = "bob.johnson@example.com", PhoneNumber = "2025550123" },
            new() { Name = "Carol", Surname = "White", Email = "carol.white@example.com", PhoneNumber = "2035550145" },
            new() { Name = "Dave", Surname = "Lee", Email = "dave.lee@example.com", PhoneNumber = "2045550167" },
            new() { Name = "Eva", Surname = "Green", Email = "eva.green@example.com", PhoneNumber = "2055550189" },
            new() { Name = "Frank", Surname = "Miller", Email = "frank.miller@example.com", PhoneNumber = "2065550202" },
            new() { Name = "Grace", Surname = "King", Email = "grace.king@example.com", PhoneNumber = "2075550224" },
            new() { Name = "Henry", Surname = "Walker", Email = "henry.walker@example.com", PhoneNumber = "2085550246" }
        };

        _context.Clients.AddRange(seed);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Seeded {Count} clients into database.", seed.Count);

        // Seed products
        if (!await _context.Products.AnyAsync(cancellationToken))
        {
            var products = new List<Domain.Entities.Product>
            {
                new() { Name = "Widget", SKU = "W-001", Price = 9.99m, Stock = 100 },
                new() { Name = "Gadget", SKU = "G-001", Price = 19.99m, Stock = 50 }
            };

            _context.Products.AddRange(products);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Seeded {Count} products.", products.Count);
        }
    }

    private async Task SeedOrdersAsync(CancellationToken cancellationToken)
    {
        // Ensure at least some orders exist for demo purposes. This method is idempotent in the sense
        // it only adds orders when existing count is below a threshold.
        var existingOrderCount = await _context.Orders.CountAsync(cancellationToken);
        if (existingOrderCount > 0)
        {
            _logger.LogDebug("Orders already seeded (count = {Count}).", existingOrderCount);
            return;
        }

        var firstClient = await _context.Clients.FirstOrDefaultAsync(cancellationToken);
        var products = await _context.Products.OrderBy(p => p.Id).ToListAsync(cancellationToken);

        if (firstClient != null && products.Any())
        {
            var product = products.First();

            var order = new Domain.Entities.Order
            {
                Client = firstClient,
                OrderDate = DateTime.UtcNow,
                Status = "Processing",
                Total = product.Price * 2
            };

            var orderItem = new Domain.Entities.OrderItem
            {
                Order = order,
                Product = product,
                Quantity = 2,
                UnitPrice = product.Price
            };

            order.OrderItems.Add(orderItem);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Created demo order {OrderId} for client {ClientId}.", order.Id, firstClient.Id);
        }

        // Add additional orders to reach at least 5 orders total
        existingOrderCount = await _context.Orders.CountAsync(cancellationToken);
        if (existingOrderCount >= 5) return;

        var clients = await _context.Clients.OrderBy(c => c.Id).Take(5).ToListAsync(cancellationToken);
        products = await _context.Products.OrderBy(p => p.Id).ToListAsync(cancellationToken);

        var additionalOrders = new List<Domain.Entities.Order>();

        foreach (var client in clients)
        {
            var o1 = new Domain.Entities.Order
            {
                Client = client,
                OrderDate = DateTime.UtcNow.AddDays(-client.Id),
                Status = (client.Id % 2 == 0) ? "Completed" : "Processing",
                Total = 0m
            };

            var p0 = products.ElementAtOrDefault(0);
            var p1 = products.ElementAtOrDefault(1);

            if (p0 != null)
            {
                var oi = new Domain.Entities.OrderItem
                {
                    Order = o1,
                    Product = p0,
                    Quantity = (int)(client.Id % 3) + 1,
                    UnitPrice = p0.Price
                };
                o1.OrderItems.Add(oi);
                o1.Total += oi.UnitPrice * oi.Quantity;
            }

            if (p1 != null && (client.Id % 2 == 0))
            {
                var oi2 = new Domain.Entities.OrderItem
                {
                    Order = o1,
                    Product = p1,
                    Quantity = 1,
                    UnitPrice = p1.Price
                };
                o1.OrderItems.Add(oi2);
                o1.Total += oi2.UnitPrice * oi2.Quantity;
            }

            additionalOrders.Add(o1);

            if (client.Id % 3 == 0)
            {
                var o2 = new Domain.Entities.Order
                {
                    Client = client,
                    OrderDate = DateTime.UtcNow.AddDays(-client.Id - 1),
                    Status = "Completed",
                    Total = 0m
                };

                if (p0 != null)
                {
                    var oi = new Domain.Entities.OrderItem
                    {
                        Order = o2,
                        Product = p0,
                        Quantity = 1,
                        UnitPrice = p0.Price
                    };
                    o2.OrderItems.Add(oi);
                    o2.Total += oi.UnitPrice * oi.Quantity;
                }

                additionalOrders.Add(o2);
            }
        }

        if (additionalOrders.Any())
        {
            _context.Orders.AddRange(additionalOrders);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Seeded {Count} additional demo orders.", additionalOrders.Count);
        }
    }
}
