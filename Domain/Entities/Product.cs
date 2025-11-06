using System.Collections.Generic;

namespace Domain.Entities;

public class Product
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string SKU { get; set; } = null!;

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
