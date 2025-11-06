using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.Entities;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DbContext = Infrastructure.DB.DbContext;

namespace Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly DbContext _context;
    private readonly ILogger<OrderService> _logger;

    public OrderService(DbContext context, ILogger<OrderService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Order>> GetOrders(CancellationToken cancellationToken)
    {
        return await _context.Orders
            .AsNoTracking()
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .ToListAsync(cancellationToken);
    }

    public async Task<Order?> GetOrderById(long id, CancellationToken cancellationToken)
    {
        return await _context.Orders
            .AsNoTracking()
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.Client)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<long> CreateOrder(Order order, CancellationToken cancellationToken)
    {
        if (order is null) throw new ArgumentNullException(nameof(order));

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);

        _logger?.LogInformation("Created order {OrderId} (ClientId: {ClientId})", order.Id, order.ClientId);
        return order.Id;
    }
}

