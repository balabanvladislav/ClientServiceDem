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

        orderGroup.MapGet("/", GetOrdersEndpoint.Handle)
            .Produces<List<Domain.DTO.Responses.OrderResponse>>()
            .WithName("GetOrders")
            .WithSummary("Get all existing orders");

        orderGroup.MapGet("/{id}", GetOrderByIdEndpoint.Handle)
            .Produces<Domain.DTO.Responses.OrderResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetOrderById")
            .WithSummary("Get an order by id with nested order items and products");

        orderGroup.MapPost("/", CreateOrderEndpoint.Handle)
            .Accepts<AddOrderRequest>(Constants.JsonContentType)
            .WithName("AddOrder")
            .WithSummary("Create a new order");
    }
}

