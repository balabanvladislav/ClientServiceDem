using System.Collections.Generic;

namespace Domain.DTO.Responses;

public class ClientResponse
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public List<OrderResponse> Orders { get; set; } = new();
}
