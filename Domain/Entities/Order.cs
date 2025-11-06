using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class Order
{
    public long Id { get; set; }

    public long ClientId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal Total { get; set; }

    public string Status { get; set; } = null!;

    // Navigation
    public Client Client { get; set; } = null!;

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
