using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Abstract;
using Domain.DTO.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Features.Endpoints.Order;

public class GetOrderByIdEndpoint
{
    public static async Task<IResult> Handle(
        long id,
        [FromServices] IOrderService orderService,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var order = await orderService.GetOrderById(id, cancellationToken);
        if (order is null) return Results.NotFound();

        var response = mapper.Map<OrderResponse>(order);
        return Results.Ok(response);
    }
}
