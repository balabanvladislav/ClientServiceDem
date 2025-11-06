namespace Domain.DTO.Requests;

public class AddOrderItemRequest
{
    public long ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }
}

