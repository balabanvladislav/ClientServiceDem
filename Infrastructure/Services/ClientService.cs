using Domain.Abstract;
using Domain.Entities;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DbContext = Infrastructure.DB.DbContext;

namespace Infrastructure.Services;

public class ClientService : IClientService
{
    private readonly DbContext _context;
    private readonly ILogger<ClientService> _logger;

    public ClientService(DbContext context, ILogger<ClientService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Client>> GetClients(CancellationToken cancellationToken)
    {
        return await _context.Clients
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Client?> GetClientById(long id, CancellationToken cancellationToken)
    {
        return await _context.Clients
            .AsNoTracking()
            .Include(c => c.Orders)
                .ThenInclude(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<long> CreateClient(Client client, CancellationToken cancellationToken)
    {
        _context.Clients.Add(client);
        await _context.SaveChangesAsync(cancellationToken);

        _logger?.LogInformation("Created client {ClientId} ({Email})", client.Id, client.Email);
        return client.Id;
    }
}