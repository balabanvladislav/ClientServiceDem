using Domain.Abstract;
using Domain.DTO.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Features.Endpoints.Client;

public class CreateClientEndpoint
{
    public static async Task<IResult> Handle(
        [FromBody] AddClientRequest request,
        IClientService clientService,
        CancellationToken cancellationToken)
    {
        if (request is null) return Results.BadRequest("Client payload is required");

        var client = new Domain.Entities.Client
        {
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };

        var newId = await clientService.CreateClient(client, cancellationToken);
        return Results.Created($"/api/clients/{newId}", newId);
    }
}
