using System;
using System.Collections.Generic;
using System.Text;


namespace Contracts.Events;

public record OrderCreatedEvent
{
    public Guid OrderId { get; init; }

    public Guid CustomerId { get; init; }

    public List<OrderItemMessage> Items { get; init; } = [];

    public DateTime CreatedAt { get; init; }
}

public record OrderItemMessage
{
    public Guid ProductId { get; init; }

    public int Quantity { get; init; }
}
