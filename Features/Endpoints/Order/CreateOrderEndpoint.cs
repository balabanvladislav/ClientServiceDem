using Domain.Abstract;
using Domain.DTO.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Features.Endpoints.Order;

public class CreateOrderEndpoint
{
    public static async Task<IResult> Handle(
        [FromBody] AddOrderRequest request,
        [FromServices] IOrderService orderService,
        CancellationToken cancellationToken)
    {
        if (request is null) return Results.BadRequest("Order payload is required");

        var order = new Domain.Entities.Order
        {
            ClientId = request.ClientId,
            OrderDate = request.OrderDate,
            Total = request.Total,
            Status = request.Status
        };

        foreach (var item in request.OrderItems)
        {
            order.OrderItems.Add(new Domain.Entities.OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            });
        }

        var newId = await orderService.CreateOrder(order, cancellationToken);
        return Results.Created($"/api/orders/{newId}", newId);
    }
}
