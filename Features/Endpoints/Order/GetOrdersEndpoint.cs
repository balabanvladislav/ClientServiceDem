using AutoMapper;
using Domain.Abstract;
using Domain.DTO.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Features.Endpoints.Order;

public class GetOrdersEndpoint
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="orderService"></param>
    /// <param name="mapper"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<IResult> Handle(
        [FromServices] IOrderService orderService,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var orders = await orderService.GetOrders(cancellationToken);
        var responses = mapper.Map<List<OrderResponse>>(orders);
        return Results.Ok(responses);
    }
}
