namespace Domain.Abstract;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Entities;

/// <summary>
/// Provides operations for managing <see cref="Client"/> entities.
/// Implementations are responsible for persistence and retrieval of client data
/// and should respect cancellation requests via <see cref="CancellationToken"/>.
/// </summary>
public interface IClientService
{
    /// <summary>
    /// Retrieves all clients from the data store.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="Client"/> instances.
    /// </returns>
    Task<List<Client>> GetClients(CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a single client by id, including related entities such as addresses and orders.
    /// </summary>
    /// <param name="id">The identifier of the client to retrieve.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the <see cref="Client"/> instance with the specified id, or <c>null</c> if not found.
    /// </returns>
    Task<Client?> GetClientById(long id, CancellationToken cancellationToken);
    
    /// <summary>
    /// Creates a new client in the data store.
    /// </summary>
    /// <param name="client">The <see cref="Client"/> instance to create. Must not be <c>null</c>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the identifier of the newly created client.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="client"/> is <c>null</c>.</exception>
    Task<long> CreateClient(Client client, CancellationToken cancellationToken);
}