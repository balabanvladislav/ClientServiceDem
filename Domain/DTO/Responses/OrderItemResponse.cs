namespace Domain.DTO.Responses;

public class OrderItemResponse
{
    public long Id { get; set; }

    public long OrderId { get; set; }

    public long ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public ProductResponse? Product { get; set; }
}

