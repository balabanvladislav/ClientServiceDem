using System;
using System.Collections.Generic;

namespace Domain.DTO.Responses;

public class OrderResponse
{
    public long Id { get; set; }

    public long ClientId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal Total { get; set; }

    public string Status { get; set; } = null!;

    public List<OrderItemResponse> OrderItems { get; set; } = new();
}
