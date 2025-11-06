using Domain.Constants;
using Domain.DTO.Requests;
using Features.Abstract;
using Features.Endpoints.Order;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Features.Endpoints;

public class OrderEndpoints : IEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        var orderGroup = app.MapGroup("/api/orders")
            .WithTags("Orders");

        orderGroup.MapGet("/", GetOrdersEndpoint.Handle);

        orderGroup.MapGet("/{id:long}", GetOrderByIdEndpoint.Handle);

        orderGroup.MapPost("/", CreateOrderEndpoint.Handle);
    }
}

