using Domain.Constants;
using Domain.DTO.Requests;
using Features.Abstract;
using Features.Endpoints.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Features.Endpoints;

public class ClientEndpoints : IEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        var clientGroup = app.MapGroup("/api/clients")
            .WithTags("Clients");

        clientGroup.MapGet("/", GetClientsEndpoint.Handle)
            .Produces<List<Domain.DTO.Responses.ClientResponse>>()
            .WithName("GetClients")
            .WithSummary("Get all existing clients");

        clientGroup.MapGet("/{id}", GetClientByIdEndpoint.Handle)
            .Produces<Domain.DTO.Responses.ClientResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetClientById")
            .WithSummary("Get a client by id with nested addresses and orders");

        clientGroup.MapPost("/", CreateClientEndpoint.Handle)
            .Accepts<AddClientRequest>(Constants.JsonContentType)
            .WithName("AddClient")
            .WithSummary("Create a new client");
    }
}
