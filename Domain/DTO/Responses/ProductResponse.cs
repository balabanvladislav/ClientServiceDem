namespace Domain.DTO.Responses;

public class ProductResponse
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string SKU { get; set; } = null!;

    public decimal Price { get; set; }

    public int Stock { get; set; }
}

