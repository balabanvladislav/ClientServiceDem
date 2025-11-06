using System.Collections.Generic;

namespace Domain.DTO.Requests;

public class AddOrderRequest
{
    public long ClientId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal Total { get; set; }

    public string Status { get; set; } = null!;

    public List<AddOrderItemRequest> OrderItems { get; set; } = new();
}

