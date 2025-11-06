using System.Collections.Generic;

namespace Domain.Entities;

public class Client
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    // Navigation
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
