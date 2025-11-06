using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Abstract;
using Domain.DTO.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Features.Endpoints.Client;

public class GetClientsEndpoint
{
    public static async Task<IResult> Handle(
        [FromServices] IClientService clientService,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var clients = await clientService.GetClients(cancellationToken);
        var responses = mapper.Map<List<ClientResponse>>(clients);
        return Results.Ok(responses);
    }
}
