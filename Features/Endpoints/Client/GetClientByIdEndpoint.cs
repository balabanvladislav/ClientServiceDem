using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Abstract;
using Domain.DTO.Responses;
using Microsoft.AspNetCore.Http;

namespace Features.Endpoints.Client;

public class GetClientByIdEndpoint
{
    public static async Task<IResult> Handle(
        long id,
        IClientService clientService,
        IMapper mapper,
        CancellationToken cancellationToken)
    {
        var client = await clientService.GetClientById(id, cancellationToken);
        if (client is null) return Results.NotFound();

        var response = mapper.Map<ClientResponse>(client);
        return Results.Ok(response);
    }
}

