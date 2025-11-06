namespace Domain.Abstract;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Entities;

public interface IOrderService
{
    Task<List<Order>> GetOrders(CancellationToken cancellationToken);

    Task<Order?> GetOrderById(long id, CancellationToken cancellationToken);

    Task<long> CreateOrder(Order order, CancellationToken cancellationToken);
}

